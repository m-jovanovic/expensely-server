using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Budgets.Contracts;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget factory.
    /// </summary>
    internal sealed class BudgetFactory : IBudgetFactory, ITransient
    {
        private readonly IBudgetDetailsValidator _budgetDetailsValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetFactory"/> class.
        /// </summary>
        /// <param name="budgetDetailsValidator">The budget details validator.</param>
        public BudgetFactory(IBudgetDetailsValidator budgetDetailsValidator) => _budgetDetailsValidator = budgetDetailsValidator;

        /// <inheritdoc />
        public Result<Budget> Create(CreateBudgetRequest createBudgetRequest)
        {
            var validateBudgetDetailsRequest = createBudgetRequest.ToValidateBudgetDetailsRequest();

            Result<IBudgetDetails> budgetDetailsResult = _budgetDetailsValidator.Validate(validateBudgetDetailsRequest);

            if (budgetDetailsResult.IsFailure)
            {
                return Result.Failure<Budget>(budgetDetailsResult.Error);
            }

            IBudgetDetails budgetDetails = budgetDetailsResult.Value;

            // TODO: Create factory method.
            var budget = new Budget(
                createBudgetRequest.User,
                budgetDetails.Name,
                budgetDetails.Money,
                budgetDetails.Categories,
                budgetDetails.StartDate,
                budgetDetails.EndDate);

            return budget;
        }
    }
}
