using System;

namespace Expensely.Application.Contracts.Budgets
{
    /// <summary>
    /// Represents the budget response.
    /// </summary>
    public sealed class BudgetResponse
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
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public int[] Categories { get; init; }

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
