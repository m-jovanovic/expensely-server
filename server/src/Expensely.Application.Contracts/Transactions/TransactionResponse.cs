using System;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public sealed class TransactionResponse
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int CategoryValue { get; init; }

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
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; init; }
    }
}
