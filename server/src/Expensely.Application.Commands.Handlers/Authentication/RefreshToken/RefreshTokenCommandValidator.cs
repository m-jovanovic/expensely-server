using Expensely.Application.Commands.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Authentication.RefreshToken
{
    /// <summary>
    /// Represents the <see cref="RefreshTokenCommand"/> validator.
    /// </summary>
    public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandValidator"/> class.
        /// </summary>
        public RefreshTokenCommandValidator() =>
            RuleFor(x => x.RefreshToken).NotEmpty().WithError(ValidationErrors.RefreshToken.RefreshTokenIsRequired);
    }
}
