export interface LanguageModel {
  code: string;
  name: string;
  flag: string;
}

export const availableLanguages: LanguageModel[] = [
  {
    code: 'en',
    name: 'English',
    flag: '.../../assets/images/en.svg'
  },
  {
    code: 'sr',
    name: 'Srpski',
    flag: '.../../assets/images/sr.svg'
  }
];
