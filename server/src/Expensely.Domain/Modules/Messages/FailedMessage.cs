using System;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Messages
{
    /// <summary>
    /// Represents a failed message that contains the original message.
    /// </summary>
    public sealed class FailedMessage : Entity, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedMessage"/> class.
        /// </summary>
        /// <param name="message">The message that has failed.</param>
        public FailedMessage(Message message) => Message = message;

        /// <summary>
        /// Gets the message.
        /// </summary>
        public Message Message { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }
    }
}
