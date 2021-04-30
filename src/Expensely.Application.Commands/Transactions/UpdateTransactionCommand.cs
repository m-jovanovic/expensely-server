using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Transactions
{
    /// <summary>
    /// Represents the command for updating a transaction.
    /// </summary>
    public sealed class UpdateTransactionCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTransactionCommand "/> class.
        /// </summary>
        /// <param name="transactionId">The user identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="occurredOn">The date the transaction occurred on.</param>
        public UpdateTransactionCommand(
            Ulid transactionId,
            string description,
            int category,
            decimal amount,
            int currency,
            DateTime occurredOn)
        {
            TransactionId = transactionId;
            Description = description;
            Category = category;
            Amount = amount;
            Currency = currency;
            OccurredOn = occurredOn;
        }

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        public Ulid TransactionId { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int Category { get; }

        /// <summary>
        /// Gets the monetary amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }

        /// <summary>
        /// Gets the date the income occurred on.
        /// </summary>
        public DateTime OccurredOn { get; }
    }
}
