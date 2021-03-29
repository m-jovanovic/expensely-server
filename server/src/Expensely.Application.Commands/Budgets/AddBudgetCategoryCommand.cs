using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Budgets
{
    /// <summary>
    /// Represents the command for adding a category to a budget.
    /// </summary>
    public sealed class AddBudgetCategoryCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddBudgetCategoryCommand"/> class.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="category">The category.</param>
        public AddBudgetCategoryCommand(Ulid budgetId, int category)
        {
            BudgetId = budgetId;
            Category = category;
        }

        /// <summary>
        /// Gets the budget identifier.
        /// </summary>
        public Ulid BudgetId { get; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int Category { get; }
    }
}
