using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Users.Commands.ChangeUserPrimaryCurrency
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPrimaryCurrencyCommand"/> validator.
    /// </summary>
    public sealed class ChangeUserPrimaryCurrencyCommandValidator : AbstractValidator<ChangeUserPrimaryCurrencyCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPrimaryCurrencyCommandValidator"/> class.
        /// </summary>
        public ChangeUserPrimaryCurrencyCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.User.IdentifierIsRequired);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(Errors.Currency.NotFound);
        }
    }
}
