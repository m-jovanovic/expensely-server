using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.UpdateIncome;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Incomes.UpdateIncome
{
    /// <summary>
    /// Represents the <see cref="UpdateIncomeCommand"/> validator.
    /// </summary>
    public sealed class UpdateIncomeCommandValidator : AbstractValidator<UpdateIncomeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateIncomeCommandValidator"/> class.
        /// </summary>
        public UpdateIncomeCommandValidator()
        {
            RuleFor(x => x.IncomeId).NotEmpty().WithError(ValidationErrors.Income.IdentifierIsRequired);

            RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.Income.NameIsRequired);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(ValidationErrors.Income.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(ValidationErrors.Income.OccurredOnDateIsRequired);
        }
    }
}
