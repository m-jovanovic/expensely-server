using System.Collections.Generic;

namespace Expensely.Contracts.Expenses
{
    /// <summary>
    /// Represents the expense list response.
    /// </summary>
    public sealed class ExpenseListResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseListResponse"/> class.
        /// </summary>
        /// <param name="items">The expenses.</param>
        /// <param name="cursor">The cursor.</param>
        public ExpenseListResponse(IReadOnlyCollection<ExpenseResponse> items, string cursor = "")
        {
            Cursor = cursor;
            Items = items;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IReadOnlyCollection<ExpenseResponse> Items { get; }

        /// <summary>
        /// Gets the cursor.
        /// </summary>
        public string Cursor { get; }
    }
}
