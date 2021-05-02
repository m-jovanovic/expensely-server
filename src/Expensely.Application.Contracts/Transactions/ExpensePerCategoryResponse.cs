namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the expense per category response.
    /// </summary>
    public sealed class ExpensePerCategoryResponse
    {
        /// <summary>
        /// Gets the category name.
        /// </summary>
        public string Category { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; init; }
    }
}
