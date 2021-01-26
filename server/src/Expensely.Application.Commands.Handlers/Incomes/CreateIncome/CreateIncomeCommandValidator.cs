using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Incomes.CreateIncome
{
    /// <summary>
    /// Represents the <see cref="CreateIncomeCommand"/> validator.
    /// </summary>
    public sealed class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user identifier provider.</param>
        public CreateIncomeCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => !string.IsNullOrWhiteSpace(x.UserId))
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.Income.NameIsRequired);

            RuleFor(x => x.Category).Must(Category.ContainsValue).WithError(ValidationErrors.Category.NotFound);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(ValidationErrors.Income.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(ValidationErrors.Income.OccurredOnDateIsRequired);
        }
    }
}
