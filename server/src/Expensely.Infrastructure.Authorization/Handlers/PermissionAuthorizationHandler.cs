using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Domain.Modules.Permissions;
using Expensely.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authorization.Handlers
{
    /// <summary>
    /// Represents the permission authorization handler.
    /// </summary>
    internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            Claim permissionClaim = context.User?.Claims?.FirstOrDefault(x => x.Type == "permissions");

            if (permissionClaim is null)
            {
                return Task.CompletedTask;
            }

            Permission[] permissions = UnpackPermissions(permissionClaim);

            if (Enum.TryParse(requirement.PermissionName, false, out Permission requiredPermission) &&
                (permissions.Contains(requiredPermission) ||
                 permissions.Contains(Permission.AccessEverything)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static Permission[] UnpackPermissions(Claim permissionClaim)
        {
            string claimValue = permissionClaim.Value;

            Permission[] permissions = claimValue.Split(',').Select(Enum.Parse<Permission>).ToArray();

            return permissions;
        }
    }
}
