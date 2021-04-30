using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Transactions;
using Expensely.Domain.Modules.Common;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Transactions.CreateTransaction
{
    /// <summary>
    /// Represents the <see cref="CreateTransactionCommand"/> validator.
    /// </summary>
    public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        public CreateTransactionCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Ulid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.Description).NotEmpty().WithError(ValidationErrors.Transaction.DescriptionIsRequired);

            RuleFor(x => x.Category).Must(Category.ContainsValue).WithError(ValidationErrors.Category.NotFound);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(ValidationErrors.Transaction.OccurredOnDateIsRequired);
        }
    }
}
