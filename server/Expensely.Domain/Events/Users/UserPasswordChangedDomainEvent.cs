﻿using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the domain event that is raised when a user password has been successfully changed.
    /// </summary>
    public sealed class UserPasswordChangedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        internal UserPasswordChangedDomainEvent(User user) => User = user;

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User { get; }
    }
}
