using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserName
{
    /// <summary>
    /// Represents the <see cref="ChangeUserNameCommand"/> validator.
    /// </summary>
    public class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserNameCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        public ChangeUserNameCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Ulid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.User.FirstNameIsRequired);

            RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.User.LastNameIsRequired);
        }
    }
}
