using System;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionDetailsById
{
    /// <summary>
    /// Represents the transaction details model.
    /// </summary>
    public sealed record TransactionDetailsModel
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; init; }

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
    }
}
