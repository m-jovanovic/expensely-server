using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for changing a user's first and lat name.
    /// </summary>
    public sealed class ChangeUserNameCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserNameCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        public ChangeUserNameCommand(Ulid userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; }
    }
}
