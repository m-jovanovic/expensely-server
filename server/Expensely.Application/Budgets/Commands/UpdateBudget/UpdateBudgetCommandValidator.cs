using Expensely.Application.Budgets.Commands.CreateBudget;
using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Budgets.Commands.UpdateBudget
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
            RuleFor(x => x.BudgetId).NotEmpty().WithError(Errors.Budget.IdentifierIsRequired);

            RuleFor(x => x.Name).NotEmpty().WithError(Errors.Budget.NameIsRequired);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(Errors.Budget.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);

            RuleFor(x => x.StartDate).NotEmpty().WithError(Errors.Budget.StartDateIsRequired);

            RuleFor(x => x.EndDate).NotEmpty().WithError(Errors.Budget.EndDateIsRequired);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .When(x => x.StartDate != default && x.EndDate != default)
                .WithError(Errors.Budget.EndDatePrecedesStartDate);
        }
    }
}
