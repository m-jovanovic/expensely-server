namespace Expensely.Api.Constants
{
    /// <summary>
    /// Contains the API endpoint routes.
    /// </summary>
    internal static class ApiRoutes
    {
        /// <summary>
        /// Contains the authentication routes.
        /// </summary>
        internal static class Authentication
        {
            /// <summary>
            /// The login route.
            /// </summary>
            internal const string Login = "authentication/login";

            /// <summary>
            /// The registration route.
            /// </summary>
            internal const string Register = "authentication/register";
        }

        /// <summary>
        /// Contains the expenses routes.
        /// </summary>
        internal static class Expenses
        {
            /// <summary>
            /// The get route.
            /// </summary>
            internal const string Get = "expenses";

            /// <summary>
            /// The create route.
            /// </summary>
            internal const string Create = "expenses";

            /// <summary>
            /// The update route.
            /// </summary>
            internal const string Update = "expenses/{id:guid}";

            /// <summary>
            /// The delete route.
            /// </summary>
            internal const string Delete = "expenses/{id:guid}";
        }
    }
}
