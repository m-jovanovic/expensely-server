using System.Collections.Generic;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Expensely.Domain.Modules.Authorization;
using Expensely.Domain.Modules.Users;

namespace Expensely.Authorization.Providers
{
    /// <summary>
    /// Represents the role provider.
    /// </summary>
    internal sealed class RoleProvider : IRoleProvider, ITransient
    {
        /// <inheritdoc />
        public IEnumerable<string> GetStandardRoles()
        {
            yield return Role.User.Name;
        }
    }
}
