export class LoadTransactionSummary {
  public static readonly type = '[TransactionSummary] Load';

  constructor(public userId: string, public currency: number) {}
}
