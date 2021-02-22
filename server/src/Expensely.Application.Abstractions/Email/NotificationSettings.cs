namespace Expensely.Application.Abstractions.Email
{
    /// <summary>
    /// Represents the notification settings.
    /// </summary>
    public sealed class NotificationSettings
    {
        /// <summary>
        /// Gets the email recipient.
        /// </summary>
        public string EmailRecipient { get; init; }
    }
}
