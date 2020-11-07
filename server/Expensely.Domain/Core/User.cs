using System;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives;
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
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public Email Email { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Verifies that the provided password hash matches the password hash.
        /// </summary>
        /// <param name="password">The password to be checked against the user password hash.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>True if the password hashes match, otherwise false.</returns>
        public bool VerifyPassword(Password password, IPasswordService passwordService)
            => !string.IsNullOrWhiteSpace(password) && passwordService.HashesMatch(password, _passwordHash);

        /// <summary>
        /// Changes the users password to the specified password.
        /// </summary>
        /// <param name="password">The new password.</param>
        /// <param name="passwordService">The password service.</param>
        /// <returns>The success result if the password was changed, otherwise an error result.</returns>
        public Result ChangePassword(Password password, IPasswordService passwordService)
        {
            string passwordHash = passwordService.Hash(password);

            if (_passwordHash == passwordHash)
            {
                return Result.Failure(DomainErrors.User.PasswordIsIdentical);
            }

            _passwordHash = passwordHash;

            // TODO: Add domain event.
            return Result.Success();
        }
    }
}
