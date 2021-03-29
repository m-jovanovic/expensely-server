using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Domain.Modules.Common;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Budgets.RemoveBudgetCategory
{
    /// <summary>
    /// Represents the <see cref="RemoveBudgetCategoryCommand"/> validator.
    /// </summary>
    public sealed class RemoveBudgetCategoryCommandValidator : AbstractValidator<RemoveBudgetCategoryCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveBudgetCategoryCommandValidator"/> class.
        /// </summary>
        public RemoveBudgetCategoryCommandValidator()
        {
            RuleFor(x => x.BudgetId).NotEmpty().WithError(ValidationErrors.Budget.IdentifierIsRequired);

            RuleFor(x => x.Category).Must(Category.ContainsValue).WithError(ValidationErrors.Category.NotFound);
        }
    }
}
