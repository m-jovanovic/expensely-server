namespace Expensely.Api.Constants
{
    /// <summary>
    /// Contains the API endpoint routes.
    /// </summary>
    internal static class ApiRoutes
    {
        /// <summary>
        /// Contains the authentication routes.
        /// </summary>
        internal static class Authentication
        {
            /// <summary>
            /// The login route.
            /// </summary>
            internal const string Login = "authentication/login";

            /// <summary>
            /// The registration route.
            /// </summary>
            internal const string Register = "authentication/register";
        }

        /// <summary>
        /// Contains the budgets routes.
        /// </summary>
        internal static class Budgets
        {
            /// <summary>
            /// The create budget route.
            /// </summary>
            internal const string CreateBudget = "budgets";

            /// <summary>
            /// The update budget route.
            /// </summary>
            internal const string UpdateBudget = "budgets/{id:guid}";

            /// <summary>
            /// The delete budget route.
            /// </summary>
            internal const string DeleteBudget = "budgets/{id:guid}";
        }

        /// <summary>
        /// Contains the expenses routes.
        /// </summary>
        internal static class Expenses
        {
            /// <summary>
            /// The get expenses route.
            /// </summary>
            internal const string GetExpenses = "expenses";

            /// <summary>
            /// The create expense route.
            /// </summary>
            internal const string CreateExpense = "expenses";

            /// <summary>
            /// The update expense route.
            /// </summary>
            internal const string UpdateExpense = "expenses/{id:guid}";

            /// <summary>
            /// The delete expense route.
            /// </summary>
            internal const string DeleteExpense = "expenses/{id:guid}";
        }

        /// <summary>
        /// Contains the currencies routes.
        /// </summary>
        internal static class Currencies
        {
            /// <summary>
            /// The get currencies route.
            /// </summary>
            internal const string GetCurrencies = "currencies";
        }

        /// <summary>
        /// Contains the users routes.
        /// </summary>
        internal static class Users
        {
            /// <summary>
            /// The add user currency route.
            /// </summary>
            internal const string AddUserCurrency = "users/{id:guid}/currencies/{currency:int}";

            /// <summary>
            /// The remove user currency route.
            /// </summary>
            internal const string RemoveUserCurrency = "users/{id:guid}/currencies/{currency:int}";

            /// <summary>
            /// The change user primary currency route.
            /// </summary>
            internal const string ChangeUserPrimaryCurrency = "users/{id}/currencies/{currency:int}/primary";
        }
    }
}
