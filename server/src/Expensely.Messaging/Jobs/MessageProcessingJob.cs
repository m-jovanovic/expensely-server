using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Messaging.Abstractions.Entities;
using Expensely.Messaging.Abstractions.Services;
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
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly ILogger<MessageProcessingJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJob"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        public MessageProcessingJob(
            IMessageDispatcher messageDispatcher,
            ILogger<MessageProcessingJob> logger)
        {
            _logger = logger;
            _messageDispatcher = messageDispatcher;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context) => await ProcessMessagesAsync(context.CancellationToken);

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            IList<Message> unprocessedMessages = new List<Message>();

            foreach (Message message in unprocessedMessages)
            {
                Maybe<Exception> maybeException = await _messageDispatcher.DispatchAsync(message, cancellationToken);

                if (maybeException.HasNoValue)
                {
                    continue;
                }

                // TODO: Register an error has occurred with the message instance.
                _logger.LogError(maybeException.Value, maybeException.Value.Message);
            }
        }
    }
}
