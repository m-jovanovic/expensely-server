export class CreateIncome {
  public static readonly type = '[Income] Create';

  constructor(public name: string, public amount: number, public currency: number, public occurredOn: Date, public description?: string) {}
}

export class UpdateIncome {
  public static readonly type = '[Income] Update';

  constructor(
    public incomeId: string,
    public name: string,
    public amount: number,
    public currency: number,
    public occurredOn: Date,
    public description?: string
  ) {}
}

export class DeleteIncome {
  public static readonly type = '[Income] Delete';

  constructor(public incomeId: string) {}
}
