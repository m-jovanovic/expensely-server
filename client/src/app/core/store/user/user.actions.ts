export class ChangePrimaryCurrency {
  public static readonly type = '[User] Change Primary Currency';

  constructor(public userId: string, public currency: number) {}
}
