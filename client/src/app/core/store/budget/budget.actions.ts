export class GetBudget {
  public static readonly type = '[Budget] Get';

  constructor(public budgetId: string) {}
}

export class CreateBudget {
  public static readonly type = '[Budget] Create';

  constructor(
    public userId: string,
    public name: string,
    public amount: number,
    public currency: number,
    public categories: number[],
    public startDate: Date,
    public endDate: Date
  ) {}
}
