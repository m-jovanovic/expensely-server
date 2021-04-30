using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the user errors.
        /// </summary>
        public static class User
        {
            /// <summary>
            /// Gets the user password is identical error.
            /// </summary>
            public static Error PasswordIsIdentical => new(
                "User.PasswordIsIdentical",
                "The specified password is identical to the user's current password.");

            /// <summary>
            /// Gets the user currency already exists error.
            /// </summary>
            public static Error CurrencyAlreadyExists => new(
                "User.CurrencyAlreadyExists",
                "The specified currency already exists in the user's currencies.");

            /// <summary>
            /// Gets the user currency does not exist error.
            /// </summary>
            public static Error CurrencyDoesNotExist => new(
                "User.CurrencyDoesNotExist",
                "The specified currency does not exist in the user's currencies.");

            /// <summary>
            /// Gets the user primary currency is identical error.
            /// </summary>
            public static Error PrimaryCurrencyIsIdentical => new(
                "User.PrimaryCurrencyIsIdentical",
                "The specified currency is identical to the user's primary currency.");

            /// <summary>
            /// Gets the user removing primary currency error.
            /// </summary>
            public static Error RemovingPrimaryCurrency => new(
                "User.RemovingPrimaryCurrency",
                "The specified currency is the user's primary currency and can not be removed.");

            /// <summary>
            /// Gets the use email is already in use error.
            /// </summary>
            public static Error EmailAlreadyInUse => new("User.EmailAlreadyInUse", "The specified email is already in use.");

            /// <summary>
            /// Gets the user email or password is invalid error.
            /// </summary>
            public static Error InvalidEmailOrPassword => new("User.InvalidEmailOrPassword", "The provided email or password is invalid.");
        }
    }
}
