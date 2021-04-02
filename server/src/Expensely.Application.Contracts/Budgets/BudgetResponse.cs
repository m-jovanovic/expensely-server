using System;
using System.Collections.Generic;
using Expensely.Application.Contracts.Categories;

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
        public Ulid Id { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; init; }

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
        public IReadOnlyCollection<CategoryResponse> Categories { get; init; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public string StartDate { get; init; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public string EndDate { get; init; }
    }
}
