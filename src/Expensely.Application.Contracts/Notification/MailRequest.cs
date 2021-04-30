namespace Expensely.Application.Contracts.Notification
{
    /// <summary>
    /// Represents a mail request.
    /// </summary>
    public sealed class MailRequest
    {
        /// <summary>
        /// Gets the recipient email.
        /// </summary>
        public string RecipientEmail { get; init; }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        public string Subject { get; init; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public string Body { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not the body is HTML.
        /// </summary>
        public bool IsBodyHtml { get; init; }
    }
}
