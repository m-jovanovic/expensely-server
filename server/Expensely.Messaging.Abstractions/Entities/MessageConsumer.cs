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
        public MessageConsumer(Message message, string consumerName)
        {
            MessageId = message.Id;
            ConsumerName = consumerName;
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Gets the message consumer.
        /// </summary>
        public string ConsumerName { get; private set; }
    }
}
