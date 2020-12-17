using System;

namespace Expensely.Domain.Reporting.Transactions
{
    /// <summary>
    /// Represents the transaction summary entity.
    /// </summary>
    public sealed class TransactionSummary
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public int Year { get; init; }

        /// <summary>
        /// Gets the month.
        /// </summary>
        public int Month { get; init; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }
    }
}
