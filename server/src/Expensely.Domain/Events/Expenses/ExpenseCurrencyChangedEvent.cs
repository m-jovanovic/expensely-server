using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when the currency of an expense is changed.
    /// </summary>
    public class ExpenseCurrencyChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; init; }

        /// <summary>
        /// Gets the previous currency.
        /// </summary>
        public int PreviousCurrency { get; init; }
    }
}
