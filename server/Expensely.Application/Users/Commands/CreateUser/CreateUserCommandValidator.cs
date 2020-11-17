using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> validator.
    /// </summary>
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandValidator"/> class.
        /// </summary>
        public CreateUserCommandValidator()
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
