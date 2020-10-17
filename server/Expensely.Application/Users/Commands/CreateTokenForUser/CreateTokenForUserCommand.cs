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
        public CreateTokenForUserCommand(string email) => Email = email;

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }
    }
}
