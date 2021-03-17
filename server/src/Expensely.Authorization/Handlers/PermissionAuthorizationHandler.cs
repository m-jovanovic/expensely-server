using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Authorization.Abstractions;
using Expensely.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authorization.Handlers
{
    /// <summary>
    /// Represents the permission authorization handler.
    /// </summary>
    internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!Enum.TryParse(requirement.PermissionName, false, out Permission requiredPermission))
            {
                return Task.CompletedTask;
            }

            IEnumerable<Permission> permissions = GetPermissions(context.User);

            if (permissions.Any(x => x == requiredPermission || x == Permission.AccessEverything))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static IEnumerable<Permission> GetPermissions(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
            {
                return Array.Empty<Permission>();
            }

            IEnumerable<Permission> permissions = claimsPrincipal.Claims
                .Where(x => x.Type == CustomJwtClaimTypes.Permissions)
                .Select(x => Enum.Parse<Permission>(x.Value));

            return permissions;
        }
    }
}
