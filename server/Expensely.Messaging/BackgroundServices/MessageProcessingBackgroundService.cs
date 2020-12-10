using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions;
using Expensely.Messaging.Infrastructure;
using Expensely.Messaging.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Expensely.Messaging.BackgroundServices
{
    /// <summary>
    /// Represents the message processing background service.
    /// </summary>
    public sealed class MessageProcessingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EventHandlerFactory _eventHandlerFactory;
        private readonly ILogger<MessageProcessingBackgroundService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingBackgroundService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public MessageProcessingBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<MessageProcessingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _eventHandlerFactory = new EventHandlerFactory();
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessMessagesAsync(stoppingToken);

                // TODO: Make the delay configurable.
                await Task.Delay(5000, stoppingToken);
            }

            await Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            IDbContext dbContext = scope.ServiceProvider.GetService<IDbContext>();

            var unprocessedMessages = dbContext
                .Set<Message>()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreatedOnUtc)
                .Take(10)
                .ToList();

            foreach (Message message in unprocessedMessages)
            {
                IEvent @event = JsonConvert.DeserializeObject<IEvent>(message.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                bool handlerFailureOccurred = false;

                foreach (object handler in _eventHandlerFactory.GetHandlers(@event, scope.ServiceProvider))
                {
                    string consumerName = handler.GetType().Name;

                    if (await dbContext.AnyAsync(new MessageConsumerSpecification(message, consumerName)))
                    {
                        continue;
                    }

                    try
                    {
                        await HandleEvent(handler, new object[] { @event, cancellationToken });
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);

                        handlerFailureOccurred = true;

                        continue;
                    }

                    dbContext.Insert(new MessageConsumer
                    {
                        MessageId = message.Id,
                        ConsumerName = consumerName
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                if (handlerFailureOccurred)
                {
                    message.RegisterFailure();
                }
                else
                {
                    message.MarkAsProcessed();
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private Task HandleEvent(object handler, object[] parameters) =>
            (Task)_eventHandlerFactory.GetHandleMethod(handler).Invoke(handler, parameters);
    }
}
