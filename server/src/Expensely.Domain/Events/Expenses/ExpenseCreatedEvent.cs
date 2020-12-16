using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when an expense is created.
    /// </summary>
    public sealed class ExpenseCreatedEvent : IEvent
    {
        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; init; }
    }
}
