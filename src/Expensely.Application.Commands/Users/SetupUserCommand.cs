using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for setting up a user.
    /// </summary>
    public sealed class SetupUserCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupUserCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        public SetupUserCommand(Ulid userId, int currency, string timeZoneId)
        {
            UserId = userId;
            Currency = currency;
            TimeZoneId = timeZoneId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; }

        /// <summary>
        /// Gets the time zone identifier.
        /// </summary>
        public string TimeZoneId { get; }
    }
}
