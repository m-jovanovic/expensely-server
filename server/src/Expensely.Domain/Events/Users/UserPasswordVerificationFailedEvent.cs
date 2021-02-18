using System;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user password verification has failed.
    /// </summary>
    public sealed class UserPasswordVerificationFailedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }
    }
}
