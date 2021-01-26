using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Users.CreateUserTokenForCredentials
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
