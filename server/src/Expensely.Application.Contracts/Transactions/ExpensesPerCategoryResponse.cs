using System.Collections.Generic;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the expenses per category response.
    /// </summary>
    public sealed class ExpensesPerCategoryResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpensesPerCategoryResponse"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public ExpensesPerCategoryResponse(IReadOnlyCollection<ExpensePerCategoryItem> items) => Items = items;

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IReadOnlyCollection<ExpensePerCategoryItem> Items { get; }

        /// <summary>
        /// Represents the <see cref="ExpensesPerCategoryResponse"/> item.
        /// </summary>
        public sealed class ExpensePerCategoryItem
        {
            /// <summary>
            /// Gets the category name.
            /// </summary>
            public string Category { get; init; }

            /// <summary>
            /// Gets the formatted amount.
            /// </summary>
            public string FormattedAmount { get; init; }
        }
    }
}
