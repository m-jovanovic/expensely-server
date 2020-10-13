using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Expenses.Commands.UpdateExpense
{
    /// <summary>
    /// Represents the command for updating an expense.
    /// </summary>
    public sealed class UpdateExpenseCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExpenseCommand"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="amount">The monetary amount.</param>
        public UpdateExpenseCommand(Guid expenseId, int currency, decimal amount)
        {
            ExpenseId = expenseId;
            Currency = currency;
            Amount = amount;
        }

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }

        /// <summary>
        /// Gets the monetary amount.
        /// </summary>
        public decimal Amount { get; }
    }
}
