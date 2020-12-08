using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the domain event that is raised when a user's primary currency has been changed.
    /// </summary>
    public sealed class UserPrimaryCurrencyChangedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPrimaryCurrencyChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        internal UserPrimaryCurrencyChangedDomainEvent(User user) => UserId = user.Id;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }
    }
}
