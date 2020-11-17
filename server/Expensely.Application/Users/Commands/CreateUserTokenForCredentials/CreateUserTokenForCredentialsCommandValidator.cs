using Expensely.Application.Extensions;
using Expensely.Application.Users.Commands.CreateUser;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Users.Commands.CreateUserTokenForCredentials
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> validator.
    /// </summary>
    public sealed class CreateUserTokenForCredentialsCommandValidator : AbstractValidator<CreateUserTokenForCredentialsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenForCredentialsCommandValidator "/> class.
        /// </summary>
        public CreateUserTokenForCredentialsCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.User.PasswordIsRequired);
        }
    }
}
