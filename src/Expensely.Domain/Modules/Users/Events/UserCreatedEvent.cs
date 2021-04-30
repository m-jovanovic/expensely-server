using System;
using Expensely.Domain.Abstractions;

namespace Expensely.Domain.Modules.Users.Events
{
    /// <summary>
    /// Represents the event that is raised when a user is created.
    /// </summary>
    public sealed class UserCreatedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; init; }
    }
}
