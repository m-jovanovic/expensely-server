using Expensely.Domain.Abstractions;

namespace Expensely.Domain.Modules.Users.Events
{
    /// <summary>
    /// Represents the event that is raised when a user password has been successfully changed.
    /// </summary>
    public sealed class UserPasswordChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; init; }
    }
}
