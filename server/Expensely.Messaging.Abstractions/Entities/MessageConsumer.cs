using System;

namespace Expensely.Messaging.Abstractions.Entities
{
    /// <summary>
    /// Represents the consumer for the specific message.
    /// </summary>
    public sealed class MessageConsumer
    {
        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        public Guid MessageId { get; init; }

        /// <summary>
        /// Gets the message consumer.
        /// </summary>
        public string ConsumerName { get; init; }
    }
}
