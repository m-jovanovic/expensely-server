export class Login {
  public static readonly type = '[Authentication] Login';

  constructor(public email: string, public password: string) {}
}

export class Logout {
  public static readonly type = '[Authentication] Logout';
}

export class Register {
  public static readonly type = '[Authentication] Register';

  constructor(
    public firstName: string,
    public lastName: string,
    public email: string,
    public password: string,
    public confirmationPassword: string
  ) {}
}

export class RefreshToken {
  public static readonly type = '[Authentication] RefreshToken';
}
