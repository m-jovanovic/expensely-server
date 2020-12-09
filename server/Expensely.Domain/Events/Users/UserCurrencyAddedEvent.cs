using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the event that is raised when a user currency has been added.
    /// </summary>
    public sealed class UserCurrencyAddedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; init; }
    }
}
