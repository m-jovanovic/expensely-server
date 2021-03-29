using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Domain.Modules.Common;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Budgets.AddBudgetCategory
{
    /// <summary>
    /// Represents the <see cref="AddBudgetCategoryCommand"/> validator.
    /// </summary>
    public sealed class AddBudgetCategoryCommandValidator : AbstractValidator<AddBudgetCategoryCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddBudgetCategoryCommandValidator"/> class.
        /// </summary>
        public AddBudgetCategoryCommandValidator()
        {
            RuleFor(x => x.BudgetId).NotEmpty().WithError(ValidationErrors.Budget.IdentifierIsRequired);

            RuleFor(x => x.Category).Must(Category.ContainsValue).WithError(ValidationErrors.Category.NotFound);
        }
    }
}
