using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when an expense is deleted.
    /// </summary>
    public sealed class ExpenseDeletedEvent : IEvent
    {
        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }
    }
}
