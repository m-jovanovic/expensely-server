using Expensely.Application.Utility;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Expenses.Commands.UpdateExpense
{
    /// <summary>
    /// Represents the <see cref="UpdateExpenseCommand"/> validator.
    /// </summary>
    public sealed class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExpenseCommandValidator"/> class.
        /// </summary>
        public UpdateExpenseCommandValidator()
        {
            RuleFor(x => x.ExpenseId).NotEmpty().WithError(Errors.Expense.IdentifierIsRequired);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);

            RuleFor(x => x.Amount).LessThanOrEqualTo(0).WithError(Errors.Expense.AmountGreaterThanZero);
        }
    }
}
