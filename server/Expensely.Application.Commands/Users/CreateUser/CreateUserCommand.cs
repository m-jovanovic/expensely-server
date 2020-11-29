using Expensely.Common.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Users.CreateUser
{
    /// <summary>
    /// Represents the command for creating a user.
    /// </summary>
    public sealed class CreateUserCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmationPassword">The confirmation password.</param>
        public CreateUserCommand(string firstName, string lastName, string email, string password, string confirmationPassword)
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
