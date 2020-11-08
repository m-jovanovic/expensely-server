using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core.Errors
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
            public static Error PasswordIsIdentical => new Error(
                "User.PasswordIsIdentical",
                "The specified password is identical to the user's current password.");

            /// <summary>
            /// Gets the user currency already exists error.
            /// </summary>
            public static Error CurrencyAlreadyExists => new Error(
                "User.CurrencyAlreadyExists",
                "The specified currency already exists in the user's currencies.");

            /// <summary>
            /// Gets the user currency does not exist error.
            /// </summary>
            public static Error CurrencyDoesNotExist => new Error(
                "User.CurrencyDoesNotExist",
                "The specified currency does not exist in the user's currencies.");

            /// <summary>
            /// Gets the user primary currency is identical error.
            /// </summary>
            public static Error PrimaryCurrencyIsIdentical => new Error(
                "User.PrimaryCurrencyIsIdentical",
                "The specified currency is identical to the user's primary currency.");

            /// <summary>
            /// Gets the user removing primary currency error.
            /// </summary>
            public static Error RemovingPrimaryCurrency => new Error(
                "User.RemovingPrimaryCurrency",
                "The specified currency is the user's primary currency and can not be removed.");
        }
    }
}
