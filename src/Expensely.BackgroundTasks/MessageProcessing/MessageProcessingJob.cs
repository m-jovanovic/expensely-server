using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.BackgroundTasks.MessageProcessing.Abstractions;
using Expensely.BackgroundTasks.MessageProcessing.Options;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Expensely.BackgroundTasks.MessageProcessing
{
    /// <summary>
    /// Represents the message processing background service.
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class MessageProcessingJob : IJob, ITransient
    {
        private readonly MessageProcessingJobOptions _options;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly ILogger<MessageProcessingJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJob"/> class.
        /// </summary>
        /// <param name="options">The message processing job options.</param>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="logger">The logger.</param>
        public MessageProcessingJob(
            IOptions<MessageProcessingJobOptions> options,
            IMessageRepository messageRepository,
            IMessageDispatcher messageDispatcher,
            ILogger<MessageProcessingJob> logger)
        {
            _options = options.Value;
            _messageRepository = messageRepository;
            _messageDispatcher = messageDispatcher;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context) => await ProcessMessagesAsync(context.CancellationToken);

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<Message> unprocessedMessages =
                    await _messageRepository.GetUnprocessedAsync(_options.BatchSize, cancellationToken);

                foreach (Message message in unprocessedMessages)
                {
                    Maybe<Exception> maybeException = await _messageDispatcher.DispatchAsync(message, cancellationToken);

                    if (maybeException.HasNoValue)
                    {
                        continue;
                    }

                    _logger.LogError(
                        maybeException.Value,
                        "Exception occurred while processing message {@MessageId}: {@ExceptionMessage}",
                        message.Id,
                        maybeException.Value.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception occurred while processing messages: {@ExceptionMessage}", ex.Message);
            }
        }
    }
}
