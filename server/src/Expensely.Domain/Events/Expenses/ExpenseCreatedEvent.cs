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
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }
    }
}
