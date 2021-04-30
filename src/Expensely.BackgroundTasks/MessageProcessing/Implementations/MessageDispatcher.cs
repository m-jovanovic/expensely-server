using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.BackgroundTasks.MessageProcessing.Abstractions;
using Expensely.BackgroundTasks.MessageProcessing.Options;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Microsoft.Extensions.Options;

namespace Expensely.BackgroundTasks.MessageProcessing.Implementations
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    internal sealed class MessageDispatcher : IMessageDispatcher, IScoped
    {
        private readonly MessageProcessingJobOptions _options;
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="options">The message processing job options.</param>
        /// <param name="eventHandlerFactory">The event handler factory.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="systemTime">The system time.</param>
        public MessageDispatcher(
            IOptions<MessageProcessingJobOptions> options,
            IEventHandlerFactory eventHandlerFactory,
            IUnitOfWork unitOfWork,
            ISystemTime systemTime)
        {
            _options = options.Value;
            _eventHandlerFactory = eventHandlerFactory;
            _unitOfWork = unitOfWork;
            _systemTime = systemTime;
        }

        /// <inheritdoc />
        public async Task<Maybe<Exception>> DispatchAsync(Message message, CancellationToken cancellationToken)
        {
            foreach (IEventHandler handler in _eventHandlerFactory.GetHandlers(message.Event))
            {
                string consumerName = handler.GetType().Name;

                if (message.HasBeenProcessedBy(consumerName))
                {
                    continue;
                }

                try
                {
                    await handler.Handle(message.Event, cancellationToken);

                    message.AddConsumer(consumerName, _systemTime.UtcNow);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    message.Retry(_options.RetryCountThreshold);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return ex;
                }
            }

            message.MarkAsProcessed();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Maybe<Exception>.None;
        }
    }
}
