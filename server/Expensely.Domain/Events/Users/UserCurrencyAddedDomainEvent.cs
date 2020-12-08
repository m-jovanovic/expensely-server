using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the domain event that is raised when a user currency has been added.
    /// </summary>
    public sealed class UserCurrencyAddedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCurrencyAddedDomainEvent"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="currency">The currency.</param>
        internal UserCurrencyAddedDomainEvent(User user, Currency currency)
            : base(Guid.NewGuid())
            => (UserId, Currency) = (user.Id, currency);

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; }
    }
}
