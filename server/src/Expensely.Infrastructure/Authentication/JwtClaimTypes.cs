namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Contains the JWT claim types used in the application.
    /// </summary>
    public static class JwtClaimTypes
    {
        /// <summary>
        /// The user identifier claim type.
        /// </summary>
        public const string UserId = "userId";

        /// <summary>
        /// The email claim type.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// The name claim type.
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// The primary currency claim type.
        /// </summary>
        public const string PrimaryCurrency = "primaryCurrency";
    }
}
