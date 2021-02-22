using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the password errors.
        /// </summary>
        public static class Password
        {
            /// <summary>
            /// Gets the password is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

            /// <summary>
            /// Gets the password is not long enough error.
            /// </summary>
            public static Error NotLongEnough => new("Password.NotLongEnough", "The password is not long enough.");

            /// <summary>
            /// Gets the password is missing uppercase letter error.
            /// </summary>
            public static Error MissingUppercaseLetter => new(
                "Password.MissingUppercaseLetter",
                "The password requires at least one uppercase letter.");

            /// <summary>
            /// Gets the password is missing lowercase letter error.
            /// </summary>
            public static Error MissingLowercaseLetter => new(
                "Password.MissingLowercaseLetter",
                "The password requires at least one lowercase letter.");

            /// <summary>
            /// Gets the password is missing number error.
            /// </summary>
            public static Error MissingNumber => new("Password.MissingNumber", "The password requires at least one number.");

            /// <summary>
            /// Gets the password is missing non-alphanumeric error.
            /// </summary>
            public static Error MissingNonAlphaNumeric => new(
                "Password.MissingNonAlphaNumeric",
                "The password requires at least one non-alphanumeric.");
        }
    }
}
