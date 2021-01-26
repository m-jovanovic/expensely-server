using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Budgets
{
    /// <summary>
    /// Represents the command for updating a budget.
    /// </summary>
    public sealed class UpdateBudgetCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBudgetCommand"/> class.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public UpdateBudgetCommand(Guid budgetId, string name, decimal amount, int currency, DateTime startDate, DateTime endDate)
        {
            BudgetId = budgetId.ToString();
            Name = name;
            Amount = amount;
            Currency = currency;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Gets the budget identifier.
        /// </summary>
        public string BudgetId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTime EndDate { get; }
    }
}
