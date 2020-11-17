using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Budgets.Commands.DeleteBudget
{
    /// <summary>
    /// Represents the <see cref="DeleteBudgetCommand"/> validator.
    /// </summary>
    public sealed class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBudgetCommandValidator"/> class.
        /// </summary>
        public DeleteBudgetCommandValidator() => RuleFor(x => x.BudgetId).NotEmpty().WithError(ValidationErrors.Budget.IdentifierIsRequired);
    }
}
