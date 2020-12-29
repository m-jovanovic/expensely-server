using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Expenses.CreateExpense
{
    /// <summary>
    /// Represents the command for creating an expense.
    /// </summary>
    public sealed class CreateExpenseCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description.</param>
        public CreateExpenseCommand(
            Guid userId,
            string name,
            int category,
            decimal amount,
            int currency,
            DateTime occurredOn,
            string description)
        {
            UserId = userId;
            Name = name;
            Category = category;
            Amount = amount;
            Currency = currency;
            OccurredOn = occurredOn;
            Description = description;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

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
        /// Gets the date the expense occurred on.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }
    }
}
