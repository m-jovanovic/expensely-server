using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Expenses.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> validator.
    /// </summary>
    public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandValidator"/> class.
        /// </summary>
        public DeleteExpenseCommandValidator() =>
            RuleFor(x => x.ExpenseId).NotEmpty().WithError(ValidationErrors.Expense.IdentifierIsRequired);
    }
}
