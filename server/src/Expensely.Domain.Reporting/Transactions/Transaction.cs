using System;

namespace Expensely.Domain.Reporting.Transactions
{
    /// <summary>
    /// Represents the transaction entitiy.
    /// </summary>
    public class Transaction
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

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }

        /// <summary>
        /// Creates a new transaction from the current transaction with the specified currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>The new transaction from the current transaction with the specified currency.</returns>
        public Transaction WithCurrency(int currency) =>
            new()
            {
                Id = Id,
                UserId = UserId,
                TransactionType = TransactionType,
                Currency = currency,
                Amount = Amount,
                OccurredOn = OccurredOn
            };
    }
}
