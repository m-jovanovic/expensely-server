using System;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction details response.
    /// </summary>
    public sealed class TransactionDetailsResponse
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
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }
    }
}
