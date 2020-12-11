using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions;
using Expensely.Messaging.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Expensely.Messaging.BackgroundServices
{
    /// <summary>
    /// Represents the message processing background service.
    /// </summary>
    public sealed class MessageProcessingJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageRepository _messageRepository;
        private readonly EventHandlerFactory _eventHandlerFactory;
        private readonly ILogger<MessageProcessingJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJob"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="logger">The logger.</param>
        public MessageProcessingJob(
            IServiceProvider serviceProvider,
            MessageRepository messageRepository,
            ILogger<MessageProcessingJob> logger)
        {
            _serviceProvider = serviceProvider;
            _messageRepository = messageRepository;
            _eventHandlerFactory = new EventHandlerFactory();
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context) => await ProcessMessagesAsync(context.CancellationToken);

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Message> unprocessedMessages = await _messageRepository.GetUnprocessedAsync(20);

            foreach (Message message in unprocessedMessages)
            {
                IEvent @event = JsonConvert.DeserializeObject<IEvent>(message.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                bool handlerFailureOccurred = false;

                using IServiceScope scope = _serviceProvider.CreateScope();

                foreach (object handler in _eventHandlerFactory.GetHandlers(@event, scope.ServiceProvider))
                {
                    string consumerName = handler.GetType().Name;

                    if (await _messageRepository.CheckIfConsumerExistsAsync(message, consumerName))
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

                    await _messageRepository.InsertConsumerAsync(new MessageConsumer
                    {
                        MessageId = message.Id,
                        ConsumerName = consumerName
                    });
                }

                if (handlerFailureOccurred)
                {
                    // TODO: Add retry mechanism, and "dead" message queue.
                    _logger.LogWarning($"Message {message.Id} encountered a handler failure.");
                }
                else
                {
                    await _messageRepository.MarkAsProcessedAsync(message);
                }
            }
        }

        private Task HandleEvent(object handler, object[] parameters) =>
            (Task)_eventHandlerFactory.GetHandleMethod(handler).Invoke(handler, parameters);
    }
}
