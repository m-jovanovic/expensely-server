using System;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the create expense request.
    /// </summary>
    public sealed class CreateTransactionRequest
    {
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
        /// Gets the currency value.
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
