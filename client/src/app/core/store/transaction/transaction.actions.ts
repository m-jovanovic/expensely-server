export class GetTransactionById {
  public static readonly type = '[Transaction] Get by Id';

  constructor(public transactionId: string) {}
}
