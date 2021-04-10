using System.Linq;
using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Domain.Modules.Common;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Budgets.UpdateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> validator.
    /// </summary>
    public sealed class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBudgetCommandValidator"/> class.
        /// </summary>
        public UpdateBudgetCommandValidator()
        {
            RuleFor(x => x.BudgetId).NotEmpty().WithError(ValidationErrors.Budget.IdentifierIsRequired);

            RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.Budget.NameIsRequired);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(ValidationErrors.Budget.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleForEach(x => x.Categories)
                .Must(Category.ContainsValue)
                .When(x => x.Categories.Any())
                .WithError(ValidationErrors.Category.NotFound);

            RuleFor(x => x.StartDate).NotEmpty().WithError(ValidationErrors.Budget.StartDateIsRequired);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithError(ValidationErrors.Budget.EndDateIsRequired)
                .GreaterThanOrEqualTo(x => x.StartDate).WithError(ValidationErrors.Budget.EndDatePrecedesStartDate);
        }
    }
}
