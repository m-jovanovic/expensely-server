using System;

namespace Expensely.Messaging.Abstractions.Entities
{
    /// <summary>
    /// Represents the consumer for the specific message.
    /// </summary>
    public sealed class MessageConsumer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="consumerName">The consumer name.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        public MessageConsumer(Message message, string consumerName, DateTime createdOnUtc)
        {
            MessageId = message.Id;
            ConsumerName = consumerName;
            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private MessageConsumer()
        {
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Gets the message consumer.
        /// </summary>
        public string ConsumerName { get; private set; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; private set; }
    }
}
