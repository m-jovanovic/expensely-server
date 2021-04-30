using System;
using System.Collections.Generic;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Messages
{
    /// <summary>
    /// Represents the consumer for the specific message.
    /// </summary>
    public sealed class MessageConsumer : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <param name="consumerName">The consumer name.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        internal MessageConsumer(string consumerName, DateTime createdOnUtc)
            : this()
        {
            ConsumerName = consumerName;
            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private MessageConsumer()
        {
        }

        /// <summary>
        /// Gets the consumer name.
        /// </summary>
        public string ConsumerName { get; private set; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ConsumerName;
        }
    }
}
