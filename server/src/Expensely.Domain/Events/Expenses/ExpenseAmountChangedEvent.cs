using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when the monetary amount of an expense is changed.
    /// </summary>
    public sealed class ExpenseAmountChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the expense.
        /// </summary>
        public Guid ExpenseId { get; init; }
    }
}
