using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Core.Errors
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
            public static Error NullOrEmpty => new Error("Password.NullOrEmpty", "The password is required.");

            /// <summary>
            /// Gets the password is not long enough error.
            /// </summary>
            public static Error NotLongEnough => new Error("Password.NotLongEnough", "The password is not long enough.");

            /// <summary>
            /// Gets the password is missing uppercase letter error.
            /// </summary>
            public static Error MissingUppercaseLetter => new Error(
                "Password.MissingUppercaseLetter",
                "The password requires at least one uppercase letter.");

            /// <summary>
            /// Gets the password is missing lowercase letter error.
            /// </summary>
            public static Error MissingLowercaseLetter => new Error(
                "Password.MissingLowercaseLetter",
                "The password requires at least one lowercase letter.");

            /// <summary>
            /// Gets the password is missing number error.
            /// </summary>
            public static Error MissingNumber => new Error("Password.MissingNumber", "The password requires at least one number.");

            /// <summary>
            /// Gets the password is missing non-alphanumeric error.
            /// </summary>
            public static Error MissingNonAlphaNumeric => new Error(
                "Password.MissingNonAlphaNumeric",
                "The password requires at least one non-alphanumeric.");
        }
    }
}
