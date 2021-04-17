using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Budgets.Contracts;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget factory interface.
    /// </summary>
    public interface IBudgetFactory
    {
        /// <summary>
        /// Creates a new transaction based on the specified <see cref="CreateBudgetRequest"/> instance.
        /// </summary>
        /// <param name="createBudgetRequest">The create budget request.</param>
        /// <returns>The result of the budget creation process containing the budget or an error.</returns>
        Result<Budget> Create(CreateBudgetRequest createBudgetRequest);
    }
}
