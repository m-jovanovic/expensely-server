using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Expensely.Messaging.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Expensely.Messaging.Jobs
{
    /// <summary>
    /// Represents the message processing background service.
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class MessageProcessingJob : IJob
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly ILogger<MessageProcessingJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJob"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="logger">The logger.</param>
        public MessageProcessingJob(
            IMessageRepository messageRepository,
            IMessageDispatcher messageDispatcher,
            ILogger<MessageProcessingJob> logger)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _messageDispatcher = messageDispatcher;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context) => await ProcessMessagesAsync(context.CancellationToken);

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            // TODO: Make number of messages configurable.
            IReadOnlyCollection<Message> unprocessedMessages = await _messageRepository.GetUnprocessedAsync(20, cancellationToken);

            foreach (Message message in unprocessedMessages)
            {
                Maybe<Exception> maybeException = await _messageDispatcher.DispatchAsync(message, cancellationToken);

                if (maybeException.HasNoValue)
                {
                    continue;
                }

                _logger.LogError(maybeException.Value, maybeException.Value.Message);
            }
        }
    }
}
