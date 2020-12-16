using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the event that is raised when an income is created.
    /// </summary>
    public sealed class IncomeCreatedEvent : IEvent
    {
        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; init; }
    }
}
