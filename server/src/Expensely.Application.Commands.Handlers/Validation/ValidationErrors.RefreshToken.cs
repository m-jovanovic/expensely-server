using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the refresh token errors.
        /// </summary>
        public static class RefreshToken
        {
            /// <summary>
            /// Gets the refresh token is required error.
            /// </summary>
            public static Error RefreshTokenIsRequired => new("RefreshToken.RefreshTokenIsRequired", "The refresh token is required.");
        }
    }
}
