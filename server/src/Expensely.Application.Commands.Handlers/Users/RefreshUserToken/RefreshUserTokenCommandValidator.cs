using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Users.RefreshUserToken
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
