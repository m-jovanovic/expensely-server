using System;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user currency has been removed.
    /// </summary>
    public sealed class UserCurrencyRemovedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }
    }
}
