using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static class DomainErrors
    {
        /// <summary>
        /// Contains the email errors.
        /// </summary>
        public static class Email
        {
            /// <summary>
            /// Gets the email is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new Error("Email.NullOrEmpty", "The email is required.");

            /// <summary>
            /// Gets the email is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("Email.LongerThanAllowed", "The email is longer than allowed.");

            /// <summary>
            /// Gets the email is in an invalid format error.
            /// </summary>
            public static Error InvalidFormat => new Error("Email.InvalidFormat", "The email format is invalid.");
        }
    }
}
