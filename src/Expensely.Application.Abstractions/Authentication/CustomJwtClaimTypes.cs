namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Contains the custom JWT claim types used in the application.
    /// </summary>
    public static class CustomJwtClaimTypes
    {
        /// <summary>
        /// The name claim type.
        /// </summary>
        public const string FullName = "full_name";

        /// <summary>
        /// The is setup complete claim type.
        /// </summary>
        public const string IsSetupComplete = "is_setup_complete";

        /// <summary>
        /// The primary currency claim type.
        /// </summary>
        public const string PrimaryCurrency = "primary_currency";

        /// <summary>
        /// The permissions claim type.
        /// </summary>
        public const string Permissions = "permissions";
    }
}
