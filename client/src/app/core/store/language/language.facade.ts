import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';

import { ChangeLanguage, SetDefaultLanguage } from './language.actions';

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
}
