using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using FluentValidation;
using TimeZoneConverter;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserTimeZone
{
    /// <summary>
    /// Represents the <see cref="ChangeUserTimeZoneCommand"/> validator.
    /// </summary>
    public sealed class ChangeUserTimeZoneCommandValidator : AbstractValidator<ChangeUserTimeZoneCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserTimeZoneCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        public ChangeUserTimeZoneCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Ulid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.TimeZoneId).NotEmpty().WithError(ValidationErrors.TimeZone.IdentifierIsRequired);

            RuleFor(x => x.TimeZoneId)
                .Must(x => TZConvert.TryGetTimeZoneInfo(x, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.TimeZoneId))
                .WithError(ValidationErrors.TimeZone.NotFound);
        }
    }
}
