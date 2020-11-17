using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Expenses.Commands.CreateExpense
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> validator.
    /// </summary>
    public sealed class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandValidator"/> class.
        /// </summary>
        public CreateExpenseCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.Expense.NameIsRequired);

            RuleFor(x => x.Amount).LessThan(0).WithError(ValidationErrors.Expense.AmountGreaterThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(ValidationErrors.Expense.OccurredOnDateIsRequired);
        }
    }
}
