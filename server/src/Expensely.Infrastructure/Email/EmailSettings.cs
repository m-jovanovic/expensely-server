namespace Expensely.Infrastructure.Email
{
    /// <summary>
    /// Represents the email settings.
    /// </summary>
    public sealed class EmailSettings
    {
        /// <summary>
        /// Gets the host.
        /// </summary>
        public string Host { get; init; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public int Port { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not to enable SSL.
        /// </summary>
        public bool EnableSsl { get; init; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; init; }
    }
}
