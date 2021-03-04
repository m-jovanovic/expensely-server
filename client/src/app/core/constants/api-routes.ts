export namespace ApiRoutes {
  export class Authentication {
    public static readonly login = 'authentication/login';
    public static readonly register = 'authentication/register';
    public static readonly refreshToken = 'authentication/refresh-token';
  }

  export class Users {
    public static readonly changePrimaryCurrency = 'users/{userId}/currencies/{currency}/primary';
  }

  export class Transactions {
    public static readonly createTransaction = 'transactions';
    public static readonly deleteTransaction = 'transactions/{transactionId}';
    public static readonly getTransactions = 'transactions';
    public static readonly getCurrentMonthTransactionSummary = 'transactions/summary/current-month';
  }

  export class Categories {
    public static readonly getCategories = 'categories';
  }

  export class Currencies {
    public static readonly getCurrencies = 'currencies';
  }
}
