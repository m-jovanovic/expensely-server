using Expensely.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authorization.Attributes
{
    /// <summary>
    /// Represents the attribute for authorizing an action based on the <see cref="Permission"/>.
    /// </summary>
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class.
        /// </summary>
        /// <param name="permission">The permission that is required to perform the action.</param>
        public HasPermissionAttribute(Permission permission)
            : base(permission.ToString())
        {
        }
    }
}
