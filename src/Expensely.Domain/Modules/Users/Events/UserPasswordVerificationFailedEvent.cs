using Expensely.Domain.Abstractions;

namespace Expensely.Domain.Modules.Users.Events
{
    /// <summary>
    /// Represents the event that is raised when a user password verification has failed.
    /// </summary>
    public sealed class UserPasswordVerificationFailedEvent : IEvent
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; init; }
    }
}
