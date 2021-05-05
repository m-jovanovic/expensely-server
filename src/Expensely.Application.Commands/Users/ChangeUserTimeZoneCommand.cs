using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for changing a user's time zone.
    /// </summary>
    public sealed class ChangeUserTimeZoneCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserTimeZoneCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        public ChangeUserTimeZoneCommand(Ulid userId, string timeZoneId)
        {
            UserId = userId;
            TimeZoneId = timeZoneId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }

        /// <summary>
        /// Gets the time zone identifier.
        /// </summary>
        public string TimeZoneId { get; }
    }
}
