using Expensely.Application.Extensions;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Users.Commands.AddUserCurrency
{
    /// <summary>
    /// Represents the <see cref="AddUserCurrencyCommand"/> validator.
    /// </summary>
    public sealed class AddUserCurrencyCommandValidator : AbstractValidator<AddUserCurrencyCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserCurrencyCommandValidator"/> class.
        /// </summary>
        public AddUserCurrencyCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);
        }
    }
}
