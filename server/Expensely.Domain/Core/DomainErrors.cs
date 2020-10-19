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

        /// <summary>
        /// Contains the first name errors.
        /// </summary>
        public static class FirstName
        {
            /// <summary>
            /// Gets the first name is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new Error("FirstName.NullOrEmpty", "The first name is required.");

            /// <summary>
            /// Gets the first name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
        }

        /// <summary>
        /// Contains the last name errors.
        /// </summary>
        public static class LastName
        {
            /// <summary>
            /// Gets the last name is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new Error("LastName.NullOrEmpty", "The last name is required.");

            /// <summary>
            /// Gets the last name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("LastName.LongerThanAllowed", "The last name is longer than allowed.");
        }

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
