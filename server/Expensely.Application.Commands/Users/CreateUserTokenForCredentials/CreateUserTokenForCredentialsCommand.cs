using Expensely.Common.Messaging;
using Expensely.Contracts.Users;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Users.CreateUserTokenForCredentials
{
    /// <summary>
    /// Represents the command for creating a JWT token for user credentials.
    /// </summary>
    public sealed class CreateUserTokenForCredentialsCommand : ICommand<Result<TokenResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenForCredentialsCommand"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public CreateUserTokenForCredentialsCommand(string email, string password)
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
