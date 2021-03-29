using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Budgets
{
    /// <summary>
    /// Represents the command for removing a category from a budget.
    /// </summary>
    public sealed class RemoveBudgetCategoryCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveBudgetCategoryCommand"/> class.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="category">The category value.</param>
        public RemoveBudgetCategoryCommand(Ulid budgetId, int category)
        {
            BudgetId = budgetId;
            Category = category;
        }

        /// <summary>
        /// Gets the budget identifier.
        /// </summary>
        public Ulid BudgetId { get; }

        /// <summary>
        /// Gets the category value.
        /// </summary>
        public int Category { get; }
    }
}
