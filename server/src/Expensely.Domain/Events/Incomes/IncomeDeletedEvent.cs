using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the event that is raised when an income is deleted.
    /// </summary>
    public sealed class IncomeDeletedEvent : IEvent
    {
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
