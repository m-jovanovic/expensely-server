using Expensely.Application.Commands.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Authentication.Login
{
    /// <summary>
    /// Represents the <see cref="RegisterCommand"/> validator.
    /// </summary>
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator "/> class.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.User.PasswordIsRequired);
        }
    }
}
