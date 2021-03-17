using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.BackgroundTasks.MessageProcessing.Factories;
using Expensely.BackgroundTasks.MessageProcessing.Settings;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Microsoft.Extensions.Options;

namespace Expensely.BackgroundTasks.MessageProcessing.Services
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    internal sealed class MessageDispatcher : IMessageDispatcher, IScoped
    {
        private readonly MessageProcessingJobSettings _settings;
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="messageProcessingJobSettingsOptions">The message processing job settings options.</param>
        /// <param name="eventHandlerFactory">The event handler factory.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="systemTime">The system time.</param>
        public MessageDispatcher(
            IOptions<MessageProcessingJobSettings> messageProcessingJobSettingsOptions,
            IEventHandlerFactory eventHandlerFactory,
            IUnitOfWork unitOfWork,
            ISystemTime systemTime)
        {
            _settings = messageProcessingJobSettingsOptions.Value;
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
                    message.Retry(_settings.RetryCountThreshold);

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
