using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Authentication
{
    /// <summary>
    /// Represents the command for creating the authentication tokens for user credentials.
    /// </summary>
    public sealed class LoginCommand : ICommand<Result<TokenResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommand"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public LoginCommand(string email, string password)
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
