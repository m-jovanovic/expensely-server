using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.CreateTokenForUser
{
    /// <summary>
    /// Represents the command for creating a JWT token for a user.
    /// </summary>
    public sealed class CreateTokenForUserCommand : ICommand<Result<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTokenForUserCommand"/> class.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password.</param>
        public CreateTokenForUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }
    }
}
