using System.Collections.Generic;

namespace Expensely.Domain.Modules.Permissions
{
    /// <summary>
    /// Represents the permission provider.
    /// </summary>
    public sealed class PermissionProvider : IPermissionProvider
    {
        /// <inheritdoc />
        public IEnumerable<Permission> GetStandardPermissions()
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
