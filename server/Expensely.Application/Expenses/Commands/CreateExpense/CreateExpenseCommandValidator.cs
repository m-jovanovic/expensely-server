using Expensely.Application.Utility;
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
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.User.IdentifierIsRequired);

            RuleFor(x => x.Amount).LessThanOrEqualTo(0).WithError(Errors.Expense.AmountGreaterThanZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(Errors.Expense.OccurredOnDateIsRequired);
        }
    }
}
