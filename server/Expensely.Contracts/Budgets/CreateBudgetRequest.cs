﻿using System;

namespace Expensely.Contracts.Budgets
{
    /// <summary>
    /// Represents the create budget request.
    /// </summary>
    public sealed class CreateBudgetRequest
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTime EndDate { get; init; }
    }
}
