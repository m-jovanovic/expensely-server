using System;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Persistence.Entities
{
    /// <summary>
    /// Represents the domain event entity.
    /// </summary>
    public sealed class DomainEvent : IAuditableEntity
    {
        /// <summary>
        /// Gets the domain event identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the domain event name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the domain event value.
        /// </summary>
        public string Value { get; init; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }
    }
}
