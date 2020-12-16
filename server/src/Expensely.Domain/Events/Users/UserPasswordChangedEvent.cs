using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user password has been successfully changed.
    /// </summary>
    public sealed class UserPasswordChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }
    }
}
