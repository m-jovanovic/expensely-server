using System.Linq;
using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Common;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget factory.
    /// </summary>
    internal sealed class BudgetFactory : IBudgetFactory, ITransient
    {
        /// <inheritdoc />
        public Result<Budget> Create(CreateBudgetRequest createBudgetRequest)
        {
            // TODO: Add domain rule about the allowed # of budgets.
            Result<Name> nameResult = Name.Create(createBudgetRequest.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure<Budget>(nameResult.Error);
            }

            Category[] categories = createBudgetRequest.Categories
                .Select(Category.FromValue)
                .Where(x => x.HasValue && x.Value.IsExpense)
                .Select(x => x.Value)
                .ToArray();

            var money = new Money(createBudgetRequest.Amount, Currency.FromValue(createBudgetRequest.Currency).Value);

            var budget = new Budget(
                createBudgetRequest.User,
                nameResult.Value,
                money,
                categories,
                createBudgetRequest.StartDate,
                createBudgetRequest.EndDate);

            return budget;
        }
    }
}
