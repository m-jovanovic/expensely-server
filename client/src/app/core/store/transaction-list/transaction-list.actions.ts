export class CreateTransaction {
  public static readonly type = '[Transaction List] Create';

  constructor(
    public userId: string,
    public description: string,
    public category: number,
    public amount: number,
    public currency: number,
    public occurredOn: Date,
    public transactionType: number
  ) {}
}

export class DeleteTransaction {
  public static readonly type = '[Transaction List] Delete';

  constructor(public transactionId: string) {}
}

export class LoadTransactions {
  public static readonly type = '[Transaction List] Load';

  constructor(public userId: string, public limit: number) {}
}

export class LoadMoreTransactions {
  public static readonly type = '[Transaction List] Load More';

  constructor(public userId: string, public limit: number) {}
}
