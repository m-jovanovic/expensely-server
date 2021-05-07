using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using Expensely.Domain.Modules.Common;
using FluentValidation;
using TimeZoneConverter;

namespace Expensely.Application.Commands.Handlers.Users.SetupUser
{
    /// <summary>
    /// Represents the <see cref="SetupUserCommand"/> validator.
    /// </summary>
    public sealed class SetupUserCommandValidator : AbstractValidator<SetupUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupUserCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user identifier provider.</param>
        public SetupUserCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Ulid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.TimeZoneId).NotEmpty().WithError(ValidationErrors.TimeZone.IdentifierIsRequired);

            RuleFor(x => x.TimeZoneId)
                .Must(x => TZConvert.TryGetTimeZoneInfo(x, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.TimeZoneId))
                .WithError(ValidationErrors.TimeZone.NotFound);
        }
    }
}
