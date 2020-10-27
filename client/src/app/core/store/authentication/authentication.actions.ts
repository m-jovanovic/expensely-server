export class Login {
  static readonly type = '[Authentication] Login';

  constructor(public email: string, public password: string) {}
}

export class Logout {
  static readonly type = '[Authentication] Logout';
}
