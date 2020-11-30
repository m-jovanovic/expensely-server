namespace Expensely.Api.Controllers.Constants
{
    /// <summary>
    /// Contains the API endpoint routes.
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// Contains the authentication routes.
        /// </summary>
        public static class Authentication
        {
            /// <summary>
            /// The login route.
            /// </summary>
            public const string Login = "authentication/login";

            /// <summary>
            /// The registration route.
            /// </summary>
            public const string Register = "authentication/register";

            /// <summary>
            /// The refresh token route.
            /// </summary>
            public const string RefreshToken = "authentication/refresh-token";
        }

        /// <summary>
        /// Contains the budgets routes.
        /// </summary>
        public static class Budgets
        {
            /// <summary>
            /// The create budget route.
            /// </summary>
            public const string CreateBudget = "budgets";

            /// <summary>
            /// The update budget route.
            /// </summary>
            public const string UpdateBudget = "budgets/{budgetId:guid}";

            /// <summary>
            /// The delete budget route.
            /// </summary>
            public const string DeleteBudget = "budgets/{budgetId:guid}";
        }

        /// <summary>
        /// Contains the transactions routes.
        /// </summary>
        public static class Transactions
        {
            /// <summary>
            /// The get current month transaction summary route.
            /// </summary>
            public const string GetCurrentMonthTransactionSummary = "transactions/summary/current-month";
        }

        /// <summary>
        /// Contains the expenses routes.
        /// </summary>
        public static class Expenses
        {
            /// <summary>
            /// The get expenses route.
            /// </summary>
            public const string GetExpenses = "expenses";

            /// <summary>
            /// The create expense route.
            /// </summary>
            public const string CreateExpense = "expenses";

            /// <summary>
            /// The update expense route.
            /// </summary>
            public const string UpdateExpense = "expenses/{expenseId:guid}";

            /// <summary>
            /// The delete expense route.
            /// </summary>
            public const string DeleteExpense = "expenses/{expenseId:guid}";
        }

        /// <summary>
        /// Contains the currencies routes.
        /// </summary>
        public static class Currencies
        {
            /// <summary>
            /// The get currencies route.
            /// </summary>
            public const string GetCurrencies = "currencies";
        }

        /// <summary>
        /// Contains the users routes.
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// The add user currency route.
            /// </summary>
            public const string AddUserCurrency = "users/{userId:guid}/currencies/{currency:int}";

            /// <summary>
            /// The remove user currency route.
            /// </summary>
            public const string RemoveUserCurrency = "users/{userId:guid}/currencies/{currency:int}";

            /// <summary>
            /// The change user primary currency route.
            /// </summary>
            public const string ChangeUserPrimaryCurrency = "users/{userId}/currencies/{currency:int}/primary";

            /// <summary>
            /// The change user password route.
            /// </summary>
            public const string ChangeUserPassword = "users/{userId}/change-password";
        }
    }
}
