using System.Threading.Tasks;
using Expensely.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Expensely.Authorization.Providers
{
    /// <summary>
    /// Represents the permission authorization policy provider.
    /// </summary>
    internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="options">The authorization options.</param>
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy authorizationPolicy = await base.GetPolicyAsync(policyName);

            if (authorizationPolicy is not null)
            {
                return authorizationPolicy;
            }

            var permissionRequirement = new PermissionRequirement(policyName);

            var authorizationPolicyBuilder = new AuthorizationPolicyBuilder();

            authorizationPolicyBuilder.AddRequirements(permissionRequirement);

            AuthorizationPolicy newAuthorizationPolicy = authorizationPolicyBuilder.Build();

            return newAuthorizationPolicy;
        }
    }
}
