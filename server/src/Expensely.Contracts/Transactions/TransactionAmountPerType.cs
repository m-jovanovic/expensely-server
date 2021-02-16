using Expensely.Domain.Modules.Transactions;

namespace Expensely.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction amount per type.
    /// </summary>
    public sealed class TransactionAmountPerType
    {
        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }
    }
}
