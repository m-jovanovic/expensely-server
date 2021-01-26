using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Incomes.DeleteIncome
{
    /// <summary>
    /// Represents the <see cref="DeleteIncomeCommand"/> validator.
    /// </summary>
    public sealed class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIncomeCommandValidator"/> class.
        /// </summary>
        public DeleteIncomeCommandValidator() =>
            RuleFor(x => x.IncomeId).NotEmpty().WithError(ValidationErrors.Income.IdentifierIsRequired);
    }
}
