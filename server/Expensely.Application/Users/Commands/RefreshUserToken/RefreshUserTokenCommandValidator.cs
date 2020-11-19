using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using FluentValidation;

namespace Expensely.Application.Users.Commands.RefreshUserToken
{
    /// <summary>
    /// Represents the <see cref="RefreshUserTokenCommand"/> validator.
    /// </summary>
    public sealed class RefreshUserTokenCommandValidator : AbstractValidator<RefreshUserTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshUserTokenCommandValidator"/> class.
        /// </summary>
        public RefreshUserTokenCommandValidator() =>
            RuleFor(x => x.RefreshToken).NotEmpty().WithError(ValidationErrors.RefreshToken.RefreshTokenIsRequired);
    }
}
