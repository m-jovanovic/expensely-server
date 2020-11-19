using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the refresh token errors.
        /// </summary>
        public static class RefreshToken
        {
            /// <summary>
            /// Gets the refresh token not found error.
            /// </summary>
            public static Error NotFound => new Error("RefreshToken.NotFound", "The refresh token was not found.");

            /// <summary>
            /// Gets the refresh token expired error.
            /// </summary>
            public static Error Expired => new Error("RefreshToken.Expired", "The refresh token has expired and can't be used.");
        }
    }
}
