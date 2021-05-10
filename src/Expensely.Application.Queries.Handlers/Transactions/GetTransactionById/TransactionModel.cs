using System;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionById
{
    /// <summary>
    /// Represents the transaction model.
    /// </summary>
    public sealed record TransactionModel
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
        public int Category { get; init; }

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
