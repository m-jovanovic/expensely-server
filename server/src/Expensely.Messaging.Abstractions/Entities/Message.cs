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
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        /// <param name="utcNow">The current and time in UTC format.</param>
        public Message(string name, string content, DateTime utcNow)
        {
            Id = Guid.NewGuid();
            Name = name;
            Content = content;
            CreatedOnUtc = utcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Message()
        {
        }

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
        public bool Processed { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>
        /// Marks the message as processed.
        /// </summary>
        public void MarkAsProcessed() => Processed = true;
    }
}
