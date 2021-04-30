using Expensely.Domain.Abstractions;

namespace Expensely.Domain.Modules.Users.Events
{
    /// <summary>
    /// Represents the event that is raised when a user currency has been added.
    /// </summary>
    public sealed class UserCurrencyAddedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }
    }
}
