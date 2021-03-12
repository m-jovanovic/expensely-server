using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages.Events;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Messages
{
    /// <summary>
    /// Represents the message that contains the event that has been published.
    /// </summary>
    public sealed class Message : AggregateRoot, IAuditableEntity
    {
        private readonly HashSet<MessageConsumer> _messageConsumers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="event">The event to be processed.</param>
        public Message(IEvent @event)
            : base(Ulid.NewUlid()) =>
            Event = @event;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Message()
        {
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        public IEvent Event { get; private set; }

        /// <summary>
        /// Gets the retry count.
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the message has been processed.
        /// </summary>
        public bool Processed { get; private set; }

        /// <summary>
        /// Gets the message consumers.
        /// </summary>
        public IReadOnlyCollection<MessageConsumer> MessageConsumers => _messageConsumers.ToArray();

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>
        /// Marks the message as processed.
        /// </summary>
        public void MarkAsProcessed() => Processed = true;

        /// <summary>
        /// Attempts to retry the message processing.
        /// </summary>
        /// <param name="retryCountThreshold">The retry count threshold.</param>
        public void Retry(int retryCountThreshold)
        {
            RetryCount++;

            if (RetryCount < retryCountThreshold)
            {
                return;
            }

            Raise(new MessageRetryCountExceededEvent
            {
                MessageId = Ulid.Parse(Id)
            });

            MarkAsProcessed();
        }

        /// <summary>
        /// Adds the consumer with the specified name.
        /// </summary>
        /// <param name="consumerName">The consumer name.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public void AddConsumer(string consumerName, DateTime utcNow) => _messageConsumers.Add(new MessageConsumer(consumerName, utcNow));

        /// <summary>
        /// Checks if the specified consumer has already processed the message.
        /// </summary>
        /// <param name="consumerName">The consumer name.</param>
        /// <returns>True if the consumer with the specified name has processed the message, otherwise false.</returns>
        public bool HasBeenProcessedBy(string consumerName) => _messageConsumers.Contains(new MessageConsumer(consumerName, default));
    }
}
