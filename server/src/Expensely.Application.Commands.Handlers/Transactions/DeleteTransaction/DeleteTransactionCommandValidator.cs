using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Transactions;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Transactions.DeleteTransaction
{
    /// <summary>
    /// Represents the <see cref="DeleteTransactionCommand"/> validator.
    /// </summary>
    public sealed class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTransactionCommandValidator"/> class.
        /// </summary>
        public DeleteTransactionCommandValidator() =>
            RuleFor(x => x.TransactionId).NotEmpty().WithError(ValidationErrors.Transaction.IdentifierIsRequired);
    }
}
