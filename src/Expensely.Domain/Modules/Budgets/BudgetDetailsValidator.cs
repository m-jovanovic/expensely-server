using System.Linq;
using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Common;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget details validator.
    /// </summary>
    internal sealed class BudgetDetailsValidator : IBudgetDetailsValidator, ITransient
    {
        /// <inheritdoc />
        public Result<IBudgetDetails> Validate(ValidateBudgetDetailsRequest validateBudgetDetailsRequest)
        {
            Result<Name> nameResult = Name.Create(validateBudgetDetailsRequest.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure<IBudgetDetails>(nameResult.Error);
            }

            Category[] categories = validateBudgetDetailsRequest.Categories
                .Select(Category.FromValue)
                .Where(x => x.HasValue && x.Value.IsExpense)
                .Select(x => x.Value)
                .ToArray();

            Currency currency = Currency.FromValue(validateBudgetDetailsRequest.Currency).Value;

            if (!validateBudgetDetailsRequest.User.HasCurrency(currency))
            {
                return Result.Failure<IBudgetDetails>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(validateBudgetDetailsRequest.Amount, currency);

            return new BudgetDetails
            {
                Name = nameResult.Value,
                Categories = categories,
                Money = money,
                StartDate = validateBudgetDetailsRequest.StartDate,
                EndDate = validateBudgetDetailsRequest.EndDate
            };
        }
    }
}
