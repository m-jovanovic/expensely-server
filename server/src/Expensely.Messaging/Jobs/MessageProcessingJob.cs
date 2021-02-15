using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Expensely.Messaging.Services;
using Expensely.Messaging.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly int _batchSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJob"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messageProcessingJobSettingsOptions">The message processing job settings options.</param>
        public MessageProcessingJob(
            IMessageRepository messageRepository,
            IMessageDispatcher messageDispatcher,
            ILogger<MessageProcessingJob> logger,
            IOptions<MessageProcessingJobSettings> messageProcessingJobSettingsOptions)
        {
            _messageRepository = messageRepository;
            _messageDispatcher = messageDispatcher;
            _logger = logger;
            _batchSize = messageProcessingJobSettingsOptions.Value.BatchSize;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context) => await ProcessMessagesAsync(context.CancellationToken);

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<Message> unprocessedMessages =
                    await _messageRepository.GetUnprocessedAsync(_batchSize, cancellationToken);

                foreach (Message message in unprocessedMessages)
                {
                    Maybe<Exception> maybeException = await _messageDispatcher.DispatchAsync(message, cancellationToken);

                    if (maybeException.HasNoValue)
                    {
                        continue;
                    }

                    _logger.LogError(
                        maybeException.Value,
                        "Exception occurred while processing {MessageId} {ExceptionMessage}",
                        message.Id,
                        maybeException.Value.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing messages {ExceptionMessage}", ex.Message);

                throw;
            }
        }
    }
}
