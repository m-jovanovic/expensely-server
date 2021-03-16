using System.Reflection;

namespace Expensely.Authorization
{
    /// <summary>
    /// Represents the authorization assembly.
    /// </summary>
    public static class AuthorizationAssembly
    {
        /// <summary>
        /// Gets the authorization assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
