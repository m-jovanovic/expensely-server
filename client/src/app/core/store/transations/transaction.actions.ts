export class GetTransactions {
  public static readonly type = '[Transactions] Get';

  constructor(public userId: string, public limit: number) {}
}
