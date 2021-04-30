using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Budgets.Contracts;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget details validator.
    /// </summary>
    public interface IBudgetDetailsValidator
    {
        /// <summary>
        /// Validates the provided budget information and returns the result of the validation.
        /// </summary>
        /// <param name="validateBudgetDetailsRequest">The validate budget details request.</param>
        /// <returns>The result of the budget validation process containing the budget details or an error.</returns>
        Result<IBudgetDetails> Validate(ValidateBudgetDetailsRequest validateBudgetDetailsRequest);
    }
}
