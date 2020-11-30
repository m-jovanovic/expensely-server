export class LoadTransactionSummary {
  public static readonly type = '[TransactionSummary] Load';

  constructor(public userId: string, public primaryCurrency: number) {}
}
