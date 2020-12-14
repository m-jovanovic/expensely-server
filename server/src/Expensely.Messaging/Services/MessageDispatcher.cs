using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Messaging.Abstractions.Entities;
using Expensely.Messaging.Abstractions.Factories;
using Expensely.Messaging.Abstractions.Services;
using Expensely.Messaging.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Expensely.Messaging.Services
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    public sealed class MessageDispatcher : IMessageDispatcher
    {
        private readonly IDbContext _dbContext;
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="eventHandlerFactory">The event handler factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public MessageDispatcher(
            IDbContext dbContext,
            IEventHandlerFactory eventHandlerFactory,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _eventHandlerFactory = eventHandlerFactory;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<Exception>> DispatchAsync(Message message, CancellationToken cancellationToken)
        {
            IEvent @event = JsonConvert.DeserializeObject<IEvent>(message.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            using IServiceScope scope = _serviceProvider.CreateScope();

            foreach (object handler in _eventHandlerFactory.GetHandlers(@event, scope.ServiceProvider))
            {
                string consumerName = handler.GetType().Name;

                if (await _dbContext.AnyAsync(new MessageConsumerSpecification(message, consumerName), cancellationToken))
                {
                    continue;
                }

                try
                {
                    await HandleEvent(handler, new object[] { @event, cancellationToken });
                }
                catch (Exception e)
                {
                    return e;
                }

                _dbContext.Insert(new MessageConsumer(message, consumerName));

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            message.MarkAsProcessed();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Maybe<Exception>.None;
        }

        private Task HandleEvent(object handler, object[] parameters) =>
            (Task)_eventHandlerFactory.GetHandleMethod(handler).Invoke(handler, parameters);
    }
}
