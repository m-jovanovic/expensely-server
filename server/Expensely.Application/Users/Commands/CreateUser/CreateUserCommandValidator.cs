using Expensely.Application.Utility;
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
            RuleFor(x => x.FirstName).NotEmpty().WithError(Errors.User.FirstNameIsRequired);

            RuleFor(x => x.LastName).NotEmpty().WithError(Errors.User.LastNameIsRequired);

            RuleFor(x => x.Email).NotEmpty().WithError(Errors.User.EmailIsRequired);

            RuleFor(x => x.Password).NotEmpty().WithError(Errors.User.PasswordIsRequired);

            RuleFor(x => x.ConfirmationPassword)
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrWhiteSpace(x.Password))
                .WithError(Errors.User.PasswordAndConfirmationPasswordMustMatch);
        }
    }
}
