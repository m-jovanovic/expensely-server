using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user's primary currency has been changed.
    /// </summary>
    public sealed class UserPrimaryCurrencyChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }
    }
}
