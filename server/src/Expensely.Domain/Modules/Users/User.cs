using System;
using System.Collections.Generic;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Events.Users;
using Expensely.Domain.Modules.Authentication;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Services;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user of the system.
    /// </summary>
    public sealed class User : AggregateRoot, IAuditableEntity
    {
        private readonly HashSet<Currency> _currencies = new();
        private Currency _primaryCurrency;
        private string _passwordHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="passwordService">The password service.</param>
        private User(FirstName firstName, LastName lastName, Email email, Password password, IPasswordService passwordService)
            : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotEmpty(email, "The user email is required.", nameof(email));
            Ensure.NotEmpty(password, "The password is required.", nameof(password));
            Ensure.NotNull(passwordService, "The password service is required.", nameof(passwordService));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            _passwordHash = passwordService.Hash(password);
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
        /// Gets the refresh token.
        /// </summary>
        public RefreshToken RefreshToken { get; private set; }

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
        public static User Create(FirstName firstName, LastName lastName, Email email, Password password, IPasswordService passwordService)
        {
            var user = new User(firstName, lastName, email, password, passwordService);

            user.Raise(new UserCreatedEvent
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email
            });

            return user;
        }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        /// <returns>The user's full name.</returns>
        public string GetFullName() => $"{FirstName.Value} {LastName.Value}";

        /// <summary>
        /// Gets the maybe instance that may contain the primary currency.
        /// </summary>
        /// <returns>The maybe instance that may contain the user's primary currency.</returns>
        public Maybe<Currency> GetPrimaryCurrency() => _primaryCurrency?.IsEmpty() ?? true ? Maybe<Currency>.None : _primaryCurrency;

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

            if (_primaryCurrency == currency)
            {
                return Result.Failure(DomainErrors.User.PrimaryCurrencyIsIdentical);
            }

            _primaryCurrency = currency;

            Raise(new UserPrimaryCurrencyChangedEvent
            {
                UserId = Guid.Parse(Id)
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

            if (_currencies.Count == 1 && (_primaryCurrency?.IsEmpty() ?? true))
            {
                _primaryCurrency = currency;
            }

            Raise(new UserCurrencyAddedEvent
            {
                UserId = Guid.Parse(Id),
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
            if (_primaryCurrency == currency)
            {
                return Result.Failure(DomainErrors.User.RemovingPrimaryCurrency);
            }

            if (!_currencies.Remove(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyDoesNotExist);
            }

            Raise(new UserCurrencyRemovedEvent
            {
                UserId = Guid.Parse(Id),
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
            if (passwordService.HashesMatch(password, _passwordHash))
            {
                return true;
            }

            Raise(new UserPasswordVerificationFailedEvent
            {
                UserId = Guid.Parse(Id)
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

            if (passwordService.HashesMatch(newPassword, _passwordHash))
            {
                return Result.Failure(DomainErrors.User.PasswordIsIdentical);
            }

            _passwordHash = passwordService.Hash(newPassword);

            Raise(new UserPasswordChangedEvent
            {
                UserId = Guid.Parse(Id)
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
