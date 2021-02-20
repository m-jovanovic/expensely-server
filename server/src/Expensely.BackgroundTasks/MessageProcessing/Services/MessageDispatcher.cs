using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.BackgroundTasks.MessageProcessing.Factories;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Modules.Messages;
using Expensely.Domain.Primitives.Maybe;

namespace Expensely.BackgroundTasks.MessageProcessing.Services
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    public sealed class MessageDispatcher : IMessageDispatcher
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IEventHandlerHandleMethodFactory _eventHandlerHandleMethodFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="eventHandlerFactory">The event handler factory.</param>
        /// <param name="eventHandlerHandleMethodFactory">The event handler handle method factory.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="systemTime">The system time.</param>
        public MessageDispatcher(
            IEventHandlerFactory eventHandlerFactory,
            IEventHandlerHandleMethodFactory eventHandlerHandleMethodFactory,
            IUnitOfWork unitOfWork,
            ISystemTime systemTime)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _unitOfWork = unitOfWork;
            _systemTime = systemTime;
            _eventHandlerHandleMethodFactory = eventHandlerHandleMethodFactory;
        }

        /// <inheritdoc />
        public async Task<Maybe<Exception>> DispatchAsync(Message message, CancellationToken cancellationToken)
        {
            foreach (object handler in _eventHandlerFactory.GetHandlers(message.Event))
            {
                string consumerName = handler.GetType().Name;

                if (message.HasBeenProcessedBy(consumerName))
                {
                    continue;
                }

                try
                {
                    await _eventHandlerHandleMethodFactory.GetHandleMethodTask(handler, new object[] { message.Event, cancellationToken });

                    message.AddConsumer(consumerName, _systemTime.UtcNow);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    message.IncrementRetryCount();

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
