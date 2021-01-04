using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user is created.
    /// </summary>
    public sealed class UserCreatedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; init; }
    }
}
