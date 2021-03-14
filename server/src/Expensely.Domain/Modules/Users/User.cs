﻿using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Authentication;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Users.Events;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user of the system.
    /// </summary>
    public sealed class User : AggregateRoot, IAuditableEntity
    {
        private readonly HashSet<Currency> _currencies = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="passwordService">The password service.</param>
        private User(FirstName firstName, LastName lastName, Email email, Password password, IPasswordService passwordService)
            : base(Ulid.NewUlid())
        {
            Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotEmpty(email, "The user email is required.", nameof(email));
            Ensure.NotEmpty(password, "The password is required.", nameof(password));
            Ensure.NotNull(passwordService, "The password service is required.", nameof(passwordService));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordService.Hash(password);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private User()
        {
        }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public FirstName FirstName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public LastName LastName { get; private set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public Email Email { get; private set; }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public string PasswordHash { get; private set; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public RefreshToken RefreshToken { get; private set; }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        public IReadOnlyCollection<Currency> Currencies => _currencies.ToList();

        /// <summary>
        /// Gets the primary currency.
        /// </summary>
        public Currency PrimaryCurrency { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>
        /// Creates a new <see cref="User"/> based on the specified parameters.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>The newly created user.</returns>
        internal static User Create(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password,
            IPasswordService passwordService)
        {
            var user = new User(firstName, lastName, email, password, passwordService);

            user.Raise(new UserCreatedEvent
            {
                UserId = Ulid.Parse(user.Id)
            });

            return user;
        }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        /// <returns>The user's full name.</returns>
        public string GetFullName() => $"{FirstName.Value} {LastName.Value}";

        /// <summary>
        /// Checks if the user's currencies contain the specified currency.
        /// </summary>
        /// <param name="currency">The currency to be checked.</param>
        /// <returns>True if the currency is within the user's currencies, otherwise false.</returns>
        public bool HasCurrency(Currency currency) => _currencies.Contains(currency);

        /// <summary>
        /// Changes the user's primary currency.
        /// </summary>
        /// <param name="currency">The new primary currency.</param>
        /// <returns>The success result if the primary currency was changed, otherwise an error result.</returns>
        public Result ChangePrimaryCurrency(Currency currency)
        {
            if (!HasCurrency(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyDoesNotExist);
            }

            if (PrimaryCurrency == currency)
            {
                return Result.Failure(DomainErrors.User.PrimaryCurrencyIsIdentical);
            }

            PrimaryCurrency = currency;

            Raise(new UserPrimaryCurrencyChangedEvent
            {
                UserId = Id
            });

            return Result.Success();
        }

        /// <summary>
        /// Adds the specified currency to the user's currencies.
        /// </summary>
        /// <param name="currency">The currency to be added.</param>
        /// <returns>The success result if the currency was added, otherwise an error result.</returns>
        public Result AddCurrency(Currency currency)
        {
            if (!_currencies.Add(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyAlreadyExists);
            }

            if (_currencies.Count == 1 && (PrimaryCurrency?.IsEmpty() ?? true))
            {
                ChangePrimaryCurrency(currency);
            }

            Raise(new UserCurrencyAddedEvent
            {
                UserId = Id,
                Currency = currency.Value
            });

            // TODO: Check domain rules to see if it is allowed to add another currency? Could be based on the subscription.
            return Result.Success();
        }

        /// <summary>
        /// Removes the specified currency from the user's currencies.
        /// </summary>
        /// <param name="currency">The currency to be removed.</param>
        /// <returns>The success result if the currency was removed, otherwise an error result.</returns>
        public Result RemoveCurrency(Currency currency)
        {
            if (PrimaryCurrency == currency)
            {
                return Result.Failure(DomainErrors.User.RemovingPrimaryCurrency);
            }

            if (!_currencies.Remove(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyDoesNotExist);
            }

            Raise(new UserCurrencyRemovedEvent
            {
                UserId = Id,
                Currency = currency.Value
            });

            // TODO: What if this is the only currency being removed?
            return Result.Success();
        }

        /// <summary>
        /// Verifies that the provided password hash matches the password hash.
        /// </summary>
        /// <param name="password">The password to be checked against the user password hash.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>True if the password hashes match, otherwise false.</returns>
        public bool VerifyPassword(Password password, IPasswordService passwordService)
        {
            if (passwordService.HashesMatch(password, PasswordHash))
            {
                return true;
            }

            Raise(new UserPasswordVerificationFailedEvent
            {
                UserId = Id
            });

            return false;
        }

        /// <summary>
        /// Changes the user's password to the specified password.
        /// </summary>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>The success result if the password was changed, otherwise an error result.</returns>
        public Result ChangePassword(Password currentPassword, Password newPassword, IPasswordService passwordService)
        {
            if (!VerifyPassword(currentPassword, passwordService))
            {
                return Result.Failure(DomainErrors.User.InvalidEmailOrPassword);
            }

            if (passwordService.HashesMatch(newPassword, PasswordHash))
            {
                return Result.Failure(DomainErrors.User.PasswordIsIdentical);
            }

            PasswordHash = passwordService.Hash(newPassword);

            Raise(new UserPasswordChangedEvent
            {
                UserId = Id
            });

            return Result.Success();
        }

        /// <summary>
        /// Changes the user's refresh token with the specified refresh token.
        /// </summary>
        /// <param name="refreshToken">The new refresh token.</param>
        public void ChangeRefreshToken(RefreshToken refreshToken) => RefreshToken = refreshToken;
    }
}
