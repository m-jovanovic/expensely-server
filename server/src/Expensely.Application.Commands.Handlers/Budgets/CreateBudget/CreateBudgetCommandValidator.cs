﻿using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Budgets.CreateBudget;
using Expensely.Application.Commands.Handlers.Extensions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Domain.Core;
using FluentValidation;

namespace Expensely.Application.Commands.Handlers.Budgets.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> validator.
    /// </summary>
    public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandValidator"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user identifier provider.</param>
        public CreateBudgetCommandValidator(IUserInformationProvider userInformationProvider)
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.User.IdentifierIsRequired);

            RuleFor(x => x.UserId)
                .Must(x => x == userInformationProvider.UserId)
                .When(x => x.UserId != Guid.Empty)
                .WithError(ValidationErrors.User.InvalidPermissions);

            RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.Budget.NameIsRequired);

            RuleFor(x => x.Amount).GreaterThan(0).WithError(ValidationErrors.Budget.AmountLessThanOrEqualToZero);

            RuleFor(x => x.Currency).Must(Currency.ContainsValue).WithError(ValidationErrors.Currency.NotFound);

            RuleFor(x => x.StartDate).NotEmpty().WithError(ValidationErrors.Budget.StartDateIsRequired);

            RuleFor(x => x.EndDate).NotEmpty().WithError(ValidationErrors.Budget.EndDateIsRequired);

            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).WithError(ValidationErrors.Budget.EndDatePrecedesStartDate);
        }
    }
}