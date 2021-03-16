using System.Collections.Generic;

namespace Expensely.Domain.Modules.Permissions
{
    /// <summary>
    /// Represents the permission provider interface.
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Gets the standard permissions.
        /// </summary>
        /// <returns>The enumerable collection of standard permissions.</returns>
        IEnumerable<Permission> GetStandardPermissions();
    }
}
