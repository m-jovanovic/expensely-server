export class AddCurrency {
  public static readonly type = '[User] Add Currency';

  constructor(public userId: string, public currency: number) {}
}

export class ChangePrimaryCurrency {
  public static readonly type = '[User] Change Primary Currency';

  constructor(public userId: string, public currency: number) {}
}
