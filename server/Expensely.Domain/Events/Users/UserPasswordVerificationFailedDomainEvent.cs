using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the domain event that is raised when a user password verification has failed.
    /// </summary>
    public sealed class UserPasswordVerificationFailedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordVerificationFailedDomainEvent"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        internal UserPasswordVerificationFailedDomainEvent(User user) => UserId = user.Id;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }
    }
}
