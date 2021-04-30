using System.Security.Claims;
using Expensely.Application.Abstractions.Authentication;

namespace Expensely.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ClaimsPrincipal"/> class.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the primary currency claim value if it exists.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The primary currency claim value if it exists, otherwise null.</returns>
        public static string GetPrimaryCurrency(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirstValue(CustomJwtClaimTypes.PrimaryCurrency);
    }
}
