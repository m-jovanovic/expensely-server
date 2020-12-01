export class CreateExpense {
  public static readonly type = '[Expense] Create';

  constructor(public name: string, public amount: number, public currency: number, public occurredOn: Date, public description?: string) {}
}

export class UpdateExpense {
  public static readonly type = '[Expense] Update';

  constructor(
    public expenseId: string,
    public name: string,
    public amount: number,
    public currency: number,
    public occurredOn: Date,
    public description?: string
  ) {}
}

export class DeleteExpense {
  public static readonly type = '[Expense] Delete';

  constructor(public expenseId: string) {}
}
