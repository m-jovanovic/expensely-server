namespace Expensely.Presentation.Api.Constants
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
            /// The get active budgets route.
            /// </summary>
            public const string GetActiveBudgets = "budgets/active";

            /// <summary>
            /// The get budget by identifier route.
            /// </summary>
            public const string GetBudgetById = "budgets/{budgetId}";

            /// <summary>
            /// The get budget by identifier route.
            /// </summary>
            public const string GetBudgetDetailsById = "budgets/{budgetId}/details";

            /// <summary>
            /// The create budget route.
            /// </summary>
            public const string CreateBudget = "budgets";

            /// <summary>
            /// The update budget route.
            /// </summary>
            public const string UpdateBudget = "budgets/{budgetId}";

            /// <summary>
            /// The delete budget route.
            /// </summary>
            public const string DeleteBudget = "budgets/{budgetId}";
        }

        /// <summary>
        /// Contains the transactions routes.
        /// </summary>
        public static class Transactions
        {
            /// <summary>
            /// The get transactions route.
            /// </summary>
            public const string GetTransactions = "transactions";

            /// <summary>
            /// The get transaction by identifier route.
            /// </summary>
            public const string GetTransactionById = "transactions/{transactionId}";

            /// <summary>
            /// The get transaction details by identifier route.
            /// </summary>
            public const string GetTransactionDetailsById = "transactions/{transactionId}/details";

            /// <summary>
            /// The get current month transaction summary route.
            /// </summary>
            public const string GetCurrentMonthTransactionSummary = "transactions/summary/current-month";

            /// <summary>
            /// The get current month expenses per category route.
            /// </summary>
            public const string GetCurrentMonthExpensesPerCategory = "transactions/expenses/per-category/current-month";

            /// <summary>
            /// The create transaction route.
            /// </summary>
            public const string CreateTransaction = "transactions";

            /// <summary>
            /// The update transaction route.
            /// </summary>
            public const string UpdateTransaction = "transactions/{transactionId}";

            /// <summary>
            /// The delete transaction route.
            /// </summary>
            public const string DeleteTransaction = "transactions/{transactionId}";
        }

        /// <summary>
        /// Contains the categories routes.
        /// </summary>
        public static class Categories
        {
            /// <summary>
            /// The get currencies route.
            /// </summary>
            public const string GetCategories = "categories";
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
            /// The get user currencies route.
            /// </summary>
            public const string GetUserCurrencies = "users/{userId}/currencies";

            /// <summary>
            /// The change user name route.
            /// </summary>
            public const string ChangeUserName = "users/{userId}/name";

            /// <summary>
            /// The add user currency route.
            /// </summary>
            public const string AddUserCurrency = "users/{userId}/currencies/{currency:int}";

            /// <summary>
            /// The remove user currency route.
            /// </summary>
            public const string RemoveUserCurrency = "users/{userId}/currencies/{currency:int}";

            /// <summary>
            /// The change user primary currency route.
            /// </summary>
            public const string ChangeUserPrimaryCurrency = "users/{userId}/currencies/{currency:int}/primary";

            /// <summary>
            /// The change user time zone route.
            /// </summary>
            public const string ChangeUserTimeZone = "users/{userId}/time-zone";

            /// <summary>
            /// The change user password route.
            /// </summary>
            public const string ChangeUserPassword = "users/{userId}/password";

            /// <summary>
            /// The setup user route.
            /// </summary>
            public const string SetupUser = "users/{userId}/setup";
        }

        /// <summary>
        /// Contains the time zones routes.
        /// </summary>
        public static class TimeZones
        {
            /// <summary>
            /// The get time zones route.
            /// </summary>
            public const string GetTimeZones = "time-zones";
        }
    }
}
