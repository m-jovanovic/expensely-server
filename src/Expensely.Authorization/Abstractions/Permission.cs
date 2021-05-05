namespace Expensely.Authorization.Abstractions
{
    /// <summary>
    /// Represents the permissions enumeration.
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// The default permission value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The user read permission.
        /// </summary>
        UserRead = 1,

        /// <summary>
        /// The user modify permission.
        /// </summary>
        UserModify = 2,

        /// <summary>
        /// The transaction read permission.
        /// </summary>
        TransactionRead = 10,

        /// <summary>
        /// The transaction modify permission.
        /// </summary>
        TransactionModify = 11,

        /// <summary>
        /// The category read permission.
        /// </summary>
        CategoryRead = 20,

        /// <summary>
        /// The currency read permission.
        /// </summary>
        CurrencyRead = 30,

        /// <summary>
        ///  The time zone read permission.
        /// </summary>
        TimeZoneRead = 35,

        /// <summary>
        /// The budget read permission.
        /// </summary>
        BudgetRead = 40,

        /// <summary>
        /// The budget modify permission.
        /// </summary>
        BudgetModify = 41,

        /// <summary>
        /// The access everything permission.
        /// </summary>
        AccessEverything = int.MaxValue
    }
}
