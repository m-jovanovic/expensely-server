import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';

import { ChangeLanguage, SetDefaultLanguage } from './language.actions';
import { LanguageSelectors } from './language.selectors';

@Injectable({
  providedIn: 'root'
})
export class LanguageFacade {
  constructor(private store: Store) {}

  setDefaultLanguage(): void {
    this.store.dispatch(new SetDefaultLanguage());
  }

  changeLanguage(language: string): void {
    this.store.dispatch(new ChangeLanguage(language));
  }

  get currentLanguage(): string {
    return this.store.selectSnapshot(LanguageSelectors.getLanguage);
  }
}
