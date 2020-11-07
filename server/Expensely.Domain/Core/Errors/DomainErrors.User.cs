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
                "The specified password is identical to the users current password.");

            /// <summary>
            /// Gets the user currency already exists error.
            /// </summary>
            public static Error CurrencyAlreadyExists => new Error(
                "User.CurrencyAlreadyExists",
                "The specified currency already exists in the users currencies.");

            /// <summary>
            /// Gets the user currency does not exist error.
            /// </summary>
            public static Error CurrencyDoesNotExist => new Error(
                "User.CurrencyDoesNotExist",
                "The specified currency does not exist in the users currencies.");
        }
    }
}
