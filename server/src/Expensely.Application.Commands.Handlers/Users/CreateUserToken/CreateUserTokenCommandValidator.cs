using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Users.CreateUserToken
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
            RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.User.PasswordIsRequired);
        }
    }
}
