using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Expenses
{
    /// <summary>
    /// Represents the command for deleting an expense.
    /// </summary>
    public sealed class DeleteExpenseCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommand"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        public DeleteExpenseCommand(Guid expenseId) => ExpenseId = expenseId.ToString();

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public string ExpenseId { get; }
    }
}
