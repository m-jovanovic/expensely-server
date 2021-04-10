using System;

namespace Expensely.Application.Contracts.Budgets
{
    /// <summary>
    /// Represents the budget details response.
    /// </summary>
    public sealed class BudgetDetailsResponse
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public string Amount { get; init; }

        /// <summary>
        /// Gets the remaining amount.
        /// </summary>
        public string RemainingAmount { get; init; }

        /// <summary>
        /// Gets the used percentage.
        /// </summary>
        public decimal UsedPercentage { get; init; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public string[] Categories { get; init; }

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
