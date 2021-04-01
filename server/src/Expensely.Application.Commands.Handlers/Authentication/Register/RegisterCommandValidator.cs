using Expensely.Application.Commands.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Authentication.Register
{
    /// <summary>
    /// Represents the <see cref="RegisterCommand"/> validator.
    /// </summary>
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
        /// </summary>
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.User.FirstNameIsRequired);

            RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.User.LastNameIsRequired);

            RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.User.PasswordIsRequired);

            RuleFor(x => x.ConfirmationPassword)
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrWhiteSpace(x.Password))
                .WithError(ValidationErrors.User.PasswordAndConfirmationPasswordMustMatch);
        }
    }
}
