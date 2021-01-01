using System;

namespace Expensely.Domain.Reporting.Transactions
{
    /// <summary>
    /// Represents the category transaction summary.
    /// </summary>
    public sealed class CategoryTransactionSummary
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
        /// Gets the category.
        /// </summary>
        public int Category { get; init; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; init; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; init; }

        /// <summary>
        /// Gets the modified on date and time in UTC format.
        /// </summary>
        public DateTime? ModifiedOnUtc { get; init; }
    }
}
