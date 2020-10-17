using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the command for creating a user.
    /// </summary>
    public sealed class CreateUserCommand : ICommand<Result<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
        /// </summary>
        /// <param name="email">The user email.</param>
        public CreateUserCommand(string email) => Email = email;

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }
    }
}
