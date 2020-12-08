using System;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Abstractions.Events
{
    /// <summary>
    /// Represents the domain event base class.
    /// </summary>
    public abstract class DomainEvent : Entity, IAuditableEntity, IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> class.
        /// </summary>
        /// <param name="id">The domain event identifier.</param>
        protected DomainEvent(Guid id)
            : base(id)
        {
        }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }
    }
}
