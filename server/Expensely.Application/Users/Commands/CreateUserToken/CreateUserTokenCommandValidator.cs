using Expensely.Application.Extensions;
using Expensely.Application.Users.Commands.CreateUser;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Users.Commands.CreateUserToken
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> validator.
    /// </summary>
    public sealed class CreateUserTokenCommandValidator : AbstractValidator<CreateUserTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenCommandValidator "/> class.
        /// </summary>
        public CreateUserTokenCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithError(Errors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(Errors.User.PasswordIsRequired);
        }
    }
}
