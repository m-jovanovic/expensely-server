using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Authentication
{
    /// <summary>
    /// Represents the command for registering a new user.
    /// </summary>
    public sealed class RegisterCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmationPassword">The confirmation password.</param>
        public RegisterCommand(string firstName, string lastName, string email, string password, string confirmationPassword)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            ConfirmationPassword = confirmationPassword;
        }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the confirmation password.
        /// </summary>
        public string ConfirmationPassword { get; }
    }
}
