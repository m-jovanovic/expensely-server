using System.Collections.Generic;
using System.Security.Claims;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the claims provider interface.
    /// </summary>
    public interface IClaimsProvider
    {
        /// <summary>
        /// Gets the claims for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The enumerable collection of claims for the specified user.</returns>
        IEnumerable<Claim> GetClaimsForUser(User user);
    }
}
