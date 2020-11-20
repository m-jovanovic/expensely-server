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

            /// <summary>
            /// The refresh token route.
            /// </summary>
            internal const string RefreshToken = "authentication/refresh-token";
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
            internal const string UpdateBudget = "budgets/{budgetId:guid}";

            /// <summary>
            /// The delete budget route.
            /// </summary>
            internal const string DeleteBudget = "budgets/{budgetId:guid}";
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
            internal const string UpdateExpense = "expenses/{expenseId:guid}";

            /// <summary>
            /// The delete expense route.
            /// </summary>
            internal const string DeleteExpense = "expenses/{expenseId:guid}";
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
            internal const string AddUserCurrency = "users/{userId:guid}/currencies/{currency:int}";

            /// <summary>
            /// The remove user currency route.
            /// </summary>
            internal const string RemoveUserCurrency = "users/{userId:guid}/currencies/{currency:int}";

            /// <summary>
            /// The change user primary currency route.
            /// </summary>
            internal const string ChangeUserPrimaryCurrency = "users/{userId}/currencies/{currency:int}/primary";
        }
    }
}
