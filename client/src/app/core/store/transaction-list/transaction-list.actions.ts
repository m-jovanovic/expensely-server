export class LoadTransactions {
  public static readonly type = '[Transaction List] Load';

  constructor(public userId: string, public limit: number) {}
}

export class LoadMoreTransactions {
  public static readonly type = '[Transaction List] Load More';

  constructor(public userId: string, public limit: number) {}
}
