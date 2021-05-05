using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Authorization.Abstractions;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Users;

namespace Expensely.Authorization.Providers
{
    /// <summary>
    /// Represents the claims provider.
    /// </summary>
    internal sealed class ClaimsProvider : IClaimsProvider, ITransient
    {
        /// <inheritdoc />
        public IEnumerable<Claim> GetClaimsForUser(User user)
        {
            yield return new Claim(JwtRegisteredClaimNames.Sub, user.Id);

            yield return new Claim(JwtRegisteredClaimNames.Email, user.Email);

            yield return new Claim(CustomJwtClaimTypes.FullName, user.GetFullName());

            yield return new Claim(CustomJwtClaimTypes.IsSetupComplete, user.IsSetupComplete().ToString());

            yield return new Claim(
                CustomJwtClaimTypes.PrimaryCurrency,
                user.PrimaryCurrency is null ? string.Empty : user.PrimaryCurrency.Value.ToString(CultureInfo.InvariantCulture));

            foreach (Permission permission in GetPermissionsForUser(user))
            {
                yield return new Claim(CustomJwtClaimTypes.Permissions, permission.ToString());
            }
        }

        private static IEnumerable<Permission> GetPermissionsForUser(User user)
        {
            var permissions = new HashSet<Permission>();

            foreach (string roleName in user.Roles)
            {
                Role role = Role.FromName(roleName).Value;

                foreach (Permission permission in role.GetPermissions())
                {
                    permissions.Add(permission);
                }
            }

            return permissions;
        }
    }
}
