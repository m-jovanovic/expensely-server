using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Incomes
{
    /// <summary>
    /// Represents the command for creating an income.
    /// </summary>
    public sealed class CreateIncomeCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description.</param>
        public CreateIncomeCommand(
            Guid userId,
            string name,
            int category,
            decimal amount,
            int currency,
            DateTime occurredOn,
            string description)
        {
            UserId = userId.ToString();
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
        public string UserId { get; }

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
        /// Gets the date the income occurred on.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }
    }
}
