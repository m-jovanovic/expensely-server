using System;

namespace Expensely.Application.Reporting.Abstractions.Contracts
{
    /// <summary>
    /// Represents the transaction details.
    /// </summary>
    public sealed class TransactionDetails
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

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

        /// <summary>
        /// Gets a new transaction details instance with the specified currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>The new transaction details instance with the specified currency.</returns>
        public TransactionDetails WithCurrency(int currency) =>
            new()
            {
                UserId = UserId,
                Amount = Amount,
                Currency = currency,
                OccurredOn = OccurredOn
            };
    }
}
