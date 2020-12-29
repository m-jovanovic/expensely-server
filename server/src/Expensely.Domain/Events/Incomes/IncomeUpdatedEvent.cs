using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the event that is raised when an income is updated.
    /// </summary>
    public sealed class IncomeUpdatedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int Category { get; init; }

        /// <summary>
        /// Gets the previous category.
        /// </summary>
        public int? PreviousCategory { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the previous amount.
        /// </summary>
        public decimal? PreviousAmount { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the previous currency.
        /// </summary>
        public int? PreviousCurrency { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }

        /// <summary>
        /// Gets the previous occurred on date.
        /// </summary>
        public DateTime? PreviousOccurredOn { get; init; }
    }
}
