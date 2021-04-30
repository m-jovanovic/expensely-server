using System;
using Expensely.Domain.Abstractions;

namespace Expensely.Domain.Modules.Messages.Events
{
    /// <summary>
    /// Represents the event that is raised when the retry count for a message is exceeded.
    /// </summary>
    public sealed class MessageRetryCountExceededEvent : IEvent
    {
        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        public Ulid MessageId { get; init; }
    }
}
