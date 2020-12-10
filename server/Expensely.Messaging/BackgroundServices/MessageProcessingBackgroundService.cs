using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Expensely.Messaging.BackgroundServices
{
    /// <summary>
    /// Represents the message processing background service.
    /// </summary>
    public sealed class MessageProcessingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingBackgroundService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public MessageProcessingBackgroundService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            IDbContext dbContext = scope.ServiceProvider.GetService<IDbContext>();

            var unprocessedMessages = dbContext!.Set<Message>()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreatedOnUtc)
                .Take(50)
                .ToList();

            foreach (Message message in unprocessedMessages)
            {
                IEvent @event = JsonConvert.DeserializeObject<IEvent>(message.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                foreach (object handler in GetHandlers(@event))
                {
                    string consumerName = handler.GetType().Name;

                    if (await dbContext.Set<MessageConsumer>()
                        .AnyAsync(x => x.MessageId == message.Id && x.ConsumerName == consumerName, stoppingToken))
                    {
                        continue;
                    }

                    await GetAwaitableTaskFromHandler(handler, new object[] { @event, stoppingToken });

                    dbContext.Insert(new MessageConsumer
                    {
                        MessageId = message.Id,
                        ConsumerName = consumerName
                    });

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                message.MarkAsProcessed();

                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }

        private static Task GetAwaitableTaskFromHandler(object handler, object[] parameters) =>
            (Task)handler!.GetType()!.GetMethod("Handle")!.Invoke(handler, parameters);

        private IEnumerable<object> GetHandlers(IEvent @event)
        {
            Type eventHandlerGenericType = typeof(IEventHandler<>).GetGenericTypeDefinition();

            Type eventHandlerInterfaceDefinition = eventHandlerGenericType.MakeGenericType(@event.GetType());

            IEnumerable<object> eventHandlers = _serviceProvider.GetServices(eventHandlerInterfaceDefinition);

            return eventHandlers;
        }
    }
}
