namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Contains the JWT claim types used in the application.
    /// </summary>
    internal static class JwtClaimTypes
    {
        /// <summary>
        /// The user identifier claim type.
        /// </summary>
        internal const string UserId = "userId";

        /// <summary>
        /// The email claim type.
        /// </summary>
        internal const string Email = "email";

        /// <summary>
        /// The name claim type.
        /// </summary>
        internal const string Name = "name";

        /// <summary>
        /// The primary currency claim type.
        /// </summary>
        internal const string PrimaryCurrency = "primaryCurrency";
    }
}
