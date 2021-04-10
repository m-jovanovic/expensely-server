export class GetBudget {
  public static readonly type = '[Budget] Get';

  constructor(public budgetId: string) {}
}

export class GetBudgetDetails {
  public static readonly type = '[Budget] Get Details';

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

export class UpdateBudget {
  public static readonly type = '[Budget] Update';

  constructor(
    public budgetId: string,
    public name: string,
    public amount: number,
    public currency: number,
    public categories: number[],
    public startDate: Date,
    public endDate: Date
  ) {}
}

export class DeleteBudget {
  public static readonly type = '[Budget] Delete';

  constructor(public budgetId: string) {}
}
