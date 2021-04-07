using System;

namespace Expensely.Infrastructure.Logging
{
    /// <summary>
    /// Represents the logging settings.
    /// </summary>
    public sealed class LoggingOptions
    {
        /// <summary>
        /// Gets or sets the expiration in days.
        /// </summary>
        public TimeSpan ExpirationInDays { get; set; }

        /// <summary>
        /// Gets or sets the error expiration in days.
        /// </summary>
        public TimeSpan ErrorExpirationInDays { get; set; }
    }
}
