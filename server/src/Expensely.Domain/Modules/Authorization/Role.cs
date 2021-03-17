using System.Collections.Generic;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Authorization
{
    /// <summary>
    /// Represents the role enumeration.
    /// </summary>
    public abstract class Role : Enumeration<Role>
    {
        /// <summary>
        /// The user role.
        /// </summary>
        public static readonly Role User = new UserRole();

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected Role(int value, string name)
            : base(value, name)
        {
        }

        /// <summary>
        /// Gets the permissions defined for the role.
        /// </summary>
        /// <returns>The enumerable collection of permissions.</returns>
        public abstract IEnumerable<Permission> GetPermissions();

        private sealed class UserRole : Role
        {
            public UserRole()
                : base(1, nameof(User))
            {
            }

            /// <inheritdoc />
            public override IEnumerable<Permission> GetPermissions()
            {
                yield return Permission.UserRead;
                yield return Permission.UserModify;
                yield return Permission.TransactionRead;
                yield return Permission.TransactionModify;
                yield return Permission.BudgetRead;
                yield return Permission.BudgetModify;
                yield return Permission.CategoryRead;
                yield return Permission.CurrencyRead;
            }
        }
    }
}
