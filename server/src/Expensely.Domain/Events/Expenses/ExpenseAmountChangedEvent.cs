﻿using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the event that is raised when the monetary amount of an expense is changed.
    /// </summary>
    public sealed class ExpenseAmountChangedEvent : IEvent
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
        /// Gets the previous amount.
        /// </summary>
        public decimal PreviousAmount { get; init; }

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
