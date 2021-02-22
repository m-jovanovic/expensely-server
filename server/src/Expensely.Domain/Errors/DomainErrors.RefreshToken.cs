using Expensely.Domain.Primitives;
using Expensely.Shared.Primitives.Errors;

namespace Expensely.Domain.Errors
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
            public static Error NotFound => new("RefreshToken.NotFound", "The refresh token was not found.");

            /// <summary>
            /// Gets the refresh token expired error.
            /// </summary>
            public static Error Expired => new("RefreshToken.Expired", "The refresh token has expired and can't be used.");
        }
    }
}
