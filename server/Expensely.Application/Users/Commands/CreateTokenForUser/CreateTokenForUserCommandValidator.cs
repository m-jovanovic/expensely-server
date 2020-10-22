using Expensely.Application.Extensions;
using Expensely.Application.Users.Commands.CreateUser;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Users.Commands.CreateTokenForUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> validator.
    /// </summary>
    public sealed class CreateTokenForUserCommandValidator : AbstractValidator<CreateTokenForUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTokenForUserCommandValidator "/> class.
        /// </summary>
        public CreateTokenForUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithError(Errors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(Errors.User.PasswordIsRequired);
        }
    }
}
