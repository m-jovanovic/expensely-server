import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Action, State, StateContext } from '@ngxs/store';

import { LanguageStateModel } from './language-state.model';
import { ChangeLanguage, SetDefaultLanguage } from './language.actions';

@State<LanguageStateModel>({
  name: 'language',
  defaults: {
    language: 'en'
  }
})
@Injectable()
export class LanguageState {
  constructor(private translocoService: TranslocoService) {}

  @Action(SetDefaultLanguage)
  setDefaultLanguage(context: StateContext<LanguageStateModel>, action: SetDefaultLanguage): void {
    this.translocoService.setActiveLang(context.getState().language);
  }

  @Action(ChangeLanguage)
  changeLanguage(context: StateContext<LanguageStateModel>, action: ChangeLanguage): void {
    context.patchState({
      language: action.language
    });

    this.translocoService.setActiveLang(action.language);
  }
}
