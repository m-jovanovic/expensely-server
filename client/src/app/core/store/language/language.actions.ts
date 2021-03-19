export class SetDefaultLanguage {
  public static readonly type = '[Language] Set Default';
}

export class ChangeLanguage {
  public static readonly type = '[Language] Change';

  constructor(public language: string) {}
}
