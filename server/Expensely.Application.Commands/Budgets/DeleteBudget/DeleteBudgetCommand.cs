using System;
using Expensely.Common.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Budgets.DeleteBudget
{
    /// <summary>
    /// Represents the command for deleting a budget.
    /// </summary>
    public sealed class DeleteBudgetCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBudgetCommand"/> class.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        public DeleteBudgetCommand(Guid budgetId) => BudgetId = budgetId;

        /// <summary>
        /// Gets the budget identifier.
        /// </summary>
        public Guid BudgetId { get; }
    }
}
