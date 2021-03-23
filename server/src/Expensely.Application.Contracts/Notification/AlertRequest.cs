namespace Expensely.Application.Contracts.Notification
{
    /// <summary>
    /// Represents an alert request.
    /// </summary>
    public sealed class AlertRequest
    {
        /// <summary>
        /// Gets the subject.
        /// </summary>
        public string Subject { get; init; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public string Body { get; init; }
    }
}
