using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Primitives;

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
