using System;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Messaging.Abstractions.Entities
{
    /// <summary>
    /// Represents the message that can be published.
    /// </summary>
    public sealed class Message : IAuditableEntity
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the message name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the serialized JSON content.
        /// </summary>
        public string Content { get; init; }

        /// <summary>
        /// Gets the number of retries.
        /// </summary>
        public int Retries { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not the message has been processed.
        /// </summary>
        public bool Processed { get; init; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }
    }
}
