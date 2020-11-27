using System.Security.Claims;
using Expensely.Infrastructure.Authentication;

namespace Expensely.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ClaimsPrincipal"/> class.
    /// </summary>
    internal static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the user identifier claim value if it exists.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The user identifier claim value if it exists, otherwise null.</returns>
        internal static string GetUserId(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirstValue(JwtClaimTypes.UserId);

        /// <summary>
        /// Gets the primary currency claim value if it exists.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The primary currency claim value if it exists, otherwise null.</returns>
        internal static string GetPrimaryCurrency(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirstValue(JwtClaimTypes.PrimaryCurrency);
    }
}
