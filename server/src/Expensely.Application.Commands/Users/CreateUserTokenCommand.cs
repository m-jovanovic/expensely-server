using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Users;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for creating a JWT token for user credentials.
    /// </summary>
    public sealed class CreateUserTokenCommand : ICommand<Result<TokenResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenCommand"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public CreateUserTokenCommand(string email, string password)
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
