export class LoadTransactions {
  public static readonly type = '[Transactions] Load';

  constructor(public userId: string, public limit: number) {}
}
