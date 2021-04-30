namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction summary response.
    /// </summary>
    public sealed class TransactionSummaryResponse
    {
        /// <summary>
        /// Gets the formatted expense.
        /// </summary>
        public string FormattedExpense { get; init; }

        /// <summary>
        /// Gets the formatted income.
        /// </summary>
        public string FormattedIncome { get; init; }
    }
}
