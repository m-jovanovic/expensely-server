namespace ApiRoutes {
  export class Authentication {
    public static readonly login = 'authentication/login';
    public static readonly register = 'authentication/register';
    public static readonly refreshToken = 'authentication/refresh-token';
  }

  export class Transactions {
    public static readonly getCurrentMonthTransactionSummary = 'transactions/summary/current-month';
  }

  export class Expenses {
    public static readonly createExpense = 'expenses';
    public static readonly updateExpense = 'expenses/{expenseId}';
    public static readonly deleteExpense = 'expenses/{expenseId}';
  }
}
