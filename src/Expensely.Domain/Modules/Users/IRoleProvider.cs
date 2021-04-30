using System.Collections.Generic;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the role provider interface.
    /// </summary>
    public interface IRoleProvider
    {
        /// <summary>
        /// Gets the collection of standard roles.
        /// </summary>
        /// <returns>The enumerable collection of roles.</returns>
        IEnumerable<string> GetStandardRoles();
    }
}
