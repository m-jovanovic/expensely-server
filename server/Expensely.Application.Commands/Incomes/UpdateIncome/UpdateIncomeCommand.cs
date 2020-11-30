using System;
using Expensely.Common.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Incomes.UpdateIncome
{
    /// <summary>
    /// Represents the command for updating an income.
    /// </summary>
    public sealed class UpdateIncomeCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateIncomeCommand"/> class.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description.</param>
        public UpdateIncomeCommand(Guid incomeId, string name, decimal amount, int currency, DateTime occurredOn, string description)
        {
            IncomeId = incomeId;
            Name = name;
            Amount = amount;
            Currency = currency;
            OccurredOn = occurredOn;
            Description = description;
        }

        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

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
