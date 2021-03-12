export class LoadUserCurrencies {
  public static readonly type = '[User] Load Currencies';

  constructor(public userId: string) {}
}

export class AddUserCurrency {
  public static readonly type = '[User] Add Currency';

  constructor(public userId: string, public currency: number) {}
}

export class ChangeUserPrimaryCurrency {
  public static readonly type = '[User] Change Primary Currency';

  constructor(public userId: string, public currency: number) {}
}
