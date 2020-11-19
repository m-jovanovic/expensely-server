using Expensely.Domain.Primitives;

namespace Expensely.Application.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the refresh token errors.
        /// </summary>
        internal static class RefreshToken
        {
            /// <summary>
            /// Gets the refresh token is required error.
            /// </summary>
            internal static Error RefreshTokenIsRequired => new Error(
                "RefreshToken.RefreshTokenIsRequired",
                "The refresh token is required.");
        }
    }
}
