export class Login {
  static readonly type = '[Authentication] Login';

  constructor(public email: string, public password: string) {}
}

export class Logout {
  static readonly type = '[Authentication] Logout';
}

export class Register {
  static readonly type = '[Authentication] Register';

  constructor(
    public firstName: string,
    public lastName: string,
    public email: string,
    public password: string,
    public confirmationPassword: string
  ) {}
}
