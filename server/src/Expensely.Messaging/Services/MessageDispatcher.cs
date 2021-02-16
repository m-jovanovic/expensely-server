using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Modules.Messages;
using Expensely.Messaging.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Messaging.Services
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    public sealed class MessageDispatcher : IMessageDispatcher
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="eventHandlerFactory">The event handler factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="systemTime">The system time.</param>
        public MessageDispatcher(
            IEventHandlerFactory eventHandlerFactory,
            IServiceProvider serviceProvider,
            IUnitOfWork unitOfWork,
            ISystemTime systemTime)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
            _systemTime = systemTime;
        }

        /// <inheritdoc />
        public async Task<Maybe<Exception>> DispatchAsync(Message message, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            foreach (object handler in _eventHandlerFactory.GetHandlers(message.Event, scope.ServiceProvider))
            {
                string consumerName = handler.GetType().Name;

                if (message.IsConsumedBy(consumerName))
                {
                    continue;
                }

                try
                {
                    await HandleEvent(handler, new object[] { message.Event, cancellationToken });
                }
                catch (Exception e)
                {
                    message.FailureToProcess();

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return e;
                }

                message.AddConsumer(consumerName, _systemTime.UtcNow);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            message.MarkAsProcessed();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Maybe<Exception>.None;
        }

        /// <summary>
        /// Handles the event for the specified event handler and the provided parameters.
        /// </summary>
        /// <param name="handler">The event handler instance.</param>
        /// <param name="parameters">The event handler parameters.</param>
        /// <returns>The task to allow awaiting the call.</returns>
        private Task HandleEvent(object handler, object[] parameters) =>
            (Task)_eventHandlerFactory
                .GetHandleMethod(
                    handler.GetType(),
                    parameters.Select(x => x.GetType()).ToArray())
                .Invoke(handler, parameters);
    }
}
