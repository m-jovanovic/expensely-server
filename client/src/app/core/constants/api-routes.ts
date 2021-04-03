export namespace ApiRoutes {
  export class Authentication {
    public static readonly login = 'authentication/login';
    public static readonly register = 'authentication/register';
    public static readonly refreshToken = 'authentication/refresh-token';
  }

  export class Users {
    public static readonly getUserCurrencies = 'users/{userId}/currencies';
    public static readonly addUserCurrency = 'users/{userId}/currencies/{currency}';
    public static readonly changeUserPrimaryCurrency = 'users/{userId}/currencies/{currency}/primary';
  }

  export class Budgets {
    public static readonly getBudget = 'budgets/{budgetId}';
    public static readonly createBudget = 'budgets';
    public static readonly updateBudget = 'budgets/{budgetId}';
  }

  export class Transactions {
    public static readonly getTransactions = 'transactions';
    public static readonly getTransaction = 'transactions/{transactionId}';
    public static readonly getCurrentMonthTransactionSummary = 'transactions/summary/current-month';
    public static readonly createTransaction = 'transactions';
    public static readonly updateTransaction = 'transactions/{transactionId}';
    public static readonly deleteTransaction = 'transactions/{transactionId}';
  }

  export class Categories {
    public static readonly getCategories = 'categories';
  }

  export class Currencies {
    public static readonly getCurrencies = 'currencies';
  }
}
