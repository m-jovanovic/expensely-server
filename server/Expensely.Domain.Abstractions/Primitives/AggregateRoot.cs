using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Abstractions.Primitives
{
    /// <summary>
    /// Represents the aggregate root.
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
        /// </summary>
        /// <param name="id">The aggregate root identifier.</param>
        protected AggregateRoot(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected AggregateRoot()
        {
        }

        /// <summary>
        /// Gets the events. This collection is readonly.
        /// </summary>
        public IReadOnlyCollection<IEvent> Events => _events.ToList();

        /// <summary>
        /// Clears all the events.
        /// </summary>
        public void ClearEvents() => _events.Clear();

        /// <summary>
        /// Raises the specified <see cref="IEvent"/>.
        /// </summary>
        /// <param name="event">The event.</param>
        protected void Raise(IEvent @event) => _events.Add(@event);
    }
}
