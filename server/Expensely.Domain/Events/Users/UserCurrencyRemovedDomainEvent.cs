using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Users
{
    /// <summary>
    /// Represents the domain event that is raised when a user currency has been removed.
    /// </summary>
    public sealed class UserCurrencyRemovedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCurrencyRemovedDomainEvent"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="currency">The currency.</param>
        internal UserCurrencyRemovedDomainEvent(User user, Currency currency) => (User, Currency) = (user, currency);

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; }
    }
}
