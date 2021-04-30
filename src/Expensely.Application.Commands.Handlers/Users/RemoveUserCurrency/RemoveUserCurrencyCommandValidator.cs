using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using Expensely.Domain.Modules.Common;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Users.RemoveUserCurrency
{
    /// <summary>
    /// Represents the <see cref="RemoveUserCurrencyCommand"/> validator.
    /// </summary>
    public sealed class RemoveUserCurrencyCommandValidator : AbstractValidator<RemoveUserCurrencyCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user identifier provider.</param>
        public RemoveUserCurrencyCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Ulid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);
        }
    }
}
