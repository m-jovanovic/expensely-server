using System;

namespace Expensely.Application.Contracts.Budgets
{
    /// <summary>
    /// Represents the budget list item response.
    /// </summary>
    public sealed class BudgetListItemResponse
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
        /// Gets the start date.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTime EndDate { get; init; }

        /// <summary>
        /// Gets the used percentage.
        /// </summary>
        public decimal UsedPercentage { get; init; }

        /// <summary>
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; init; }
    }
}
