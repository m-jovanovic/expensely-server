export class LoadExpensesPerCategory {
  public static readonly type = '[ExpensesPerCategory] Load';

  constructor(public userId: string, public currency: number) {}
}
