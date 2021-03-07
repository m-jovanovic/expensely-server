export class GetTransaction {
  public static readonly type = '[Transaction] Get';

  constructor(public transactionId: string) {}
}

export class DeleteTransaction {
  public static readonly type = '[Transaction] Delete';

  constructor(public transactionId: string) {}
}
