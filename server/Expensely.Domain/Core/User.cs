using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using Expensely.Domain.Services;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the user of the system.
    /// </summary>
    public sealed class User : AggregateRoot, IAuditableEntity
    {
        private readonly HashSet<Currency> _currencies = new HashSet<Currency>();
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
        public User(FirstName firstName, LastName lastName, Email email, Password password, IPasswordService passwordService)
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
        /// Required by EF Core.
        /// </remarks>
        private User() => _primaryCurrency = Currency.None;

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public FirstName FirstName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public LastName LastName { get; private set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public Email Email { get; private set; }

        /// <summary>
        /// Gets the maybe instance that may contain the primary currency.
        /// </summary>
        public Maybe<Currency> PrimaryCurrency => _primaryCurrency == Currency.None ? Maybe<Currency>.None : _primaryCurrency;

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        public IReadOnlyCollection<Currency> Currencies => _currencies.ToList();

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Changes the users primary currency.
        /// </summary>
        /// <param name="currency">The new primary currency.</param>
        /// <returns>The success result if the primary currency was changed, otherwise an error result.</returns>
        public Result ChangePrimaryCurrency(Currency currency)
        {
            if (!_currencies.Contains(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyDoesNotExist);
            }

            if (_primaryCurrency == currency)
            {
                return Result.Failure(DomainErrors.User.PrimaryCurrencyIsIdentical);
            }

            _primaryCurrency = currency;

            // TODO: Add domain event.
            return Result.Success();
        }

        /// <summary>
        /// Adds the specified currency to the users currencies.
        /// </summary>
        /// <param name="currency">The currency to be added.</param>
        /// <returns>The success result if the currency was added, otherwise an error result.</returns>
        public Result AddCurrency(Currency currency)
        {
            if (!_currencies.Add(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyAlreadyExists);
            }

            if (_currencies.Count == 1 && _primaryCurrency == Currency.None)
            {
                _primaryCurrency = currency;
            }

            // TODO: Check domain rules to see if it is allowed to add another currency? Could be based on the subscription.
            // TODO: Add domain event.
            return Result.Success();
        }

        /// <summary>
        /// Removes the specified currency from the users currencies.
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

            // TODO: What if this is the only currency being removed?
            // TODO: Add domain event.
            return Result.Success();
        }

        /// <summary>
        /// Verifies that the provided password hash matches the password hash.
        /// </summary>
        /// <param name="password">The password to be checked against the user password hash.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>True if the password hashes match, otherwise false.</returns>
        public bool VerifyPassword(Password password, IPasswordService passwordService)
            => !string.IsNullOrWhiteSpace(password) && passwordService.HashesMatch(password, _passwordHash);

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

            // TODO: Add domain event.
            return Result.Success();
        }
    }
}
