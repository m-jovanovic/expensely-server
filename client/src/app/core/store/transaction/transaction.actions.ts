export class GetTransaction {
  public static readonly type = '[Transaction] Get';

  constructor(public transactionId: string) {}
}

export class GetTransactionDetails {
  public static readonly type = '[Transaction] Get Details';

  constructor(public transactionId: string) {}
}

export class CreateTransaction {
  public static readonly type = '[Transaction] Create';

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

export class UpdateTransaction {
  public static readonly type = '[Transaction] Update';

  constructor(
    public transactionId: string,
    public description: string,
    public category: number,
    public amount: number,
    public currency: number,
    public occurredOn: Date
  ) {}
}

export class DeleteTransaction {
  public static readonly type = '[Transaction] Delete';

  constructor(public transactionId: string) {}
}
