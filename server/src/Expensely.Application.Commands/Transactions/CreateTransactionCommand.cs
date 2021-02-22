using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Shared.Primitives.Result;

namespace Expensely.Application.Commands.Transactions
{
    /// <summary>
    /// Represents the command for creating a transaction.
    /// </summary>
    public sealed class CreateTransactionCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="occurredOn">The date the transaction occurred on.</param>
        /// <param name="transactionType">The transaction type.</param>
        public CreateTransactionCommand(
            Guid userId,
            string description,
            int category,
            decimal amount,
            int currency,
            DateTime occurredOn,
            int transactionType)
        {
            UserId = userId.ToString();
            Description = description;
            Category = category;
            Amount = amount;
            Currency = currency;
            OccurredOn = occurredOn;
            TransactionType = transactionType;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; }

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

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; }
    }
}
