using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Budgets.Commands.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> validator.
    /// </summary>
    public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandValidator"/> class.
        /// </summary>
        public CreateBudgetCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.User.IdentifierIsRequired);

            RuleFor(x => x.Name).NotEmpty().WithError(Errors.Budget.NameIsRequired);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(Errors.Budget.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);

            RuleFor(x => x.StartDate).NotEmpty().WithError(Errors.Budget.StartDateIsRequired);

            RuleFor(x => x.EndDate).NotEmpty().WithError(Errors.Budget.EndDateIsRequired);

            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).WithError(Errors.Budget.EndDatePrecedesStartDate);
        }
    }
}
