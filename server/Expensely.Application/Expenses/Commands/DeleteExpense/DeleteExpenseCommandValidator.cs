using Expensely.Application.Extensions;
using Expensely.Application.Utility;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Expenses.Commands.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> validator.
    /// </summary>
    public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandValidator"/> class.
        /// </summary>
        public DeleteExpenseCommandValidator() => RuleFor(x => x.ExpenseId).NotEmpty().WithError(ValidationErrors.Expense.IdentifierIsRequired);
    }
}
