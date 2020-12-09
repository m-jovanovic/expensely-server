using System;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when the monetary amount of an expense is changed.
    /// </summary>
    public sealed class ExpenseMoneyChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the expense.
        /// </summary>
        public Guid ExpenseId { get; init; }
    }
}
