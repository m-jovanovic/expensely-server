using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Users.Commands.RemoveUserCurrency
{
    /// <summary>
    /// Represents the <see cref="RemoveUserCurrencyCommand"/> validator.
    /// </summary>
    public sealed class RemoveUserCurrencyCommandValidator : AbstractValidator<RemoveUserCurrencyCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommandValidator"/> class.
        /// </summary>
        public RemoveUserCurrencyCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.User.IdentifierIsRequired);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);
        }
    }
}
