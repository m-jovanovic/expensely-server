import { Selector } from '@ngxs/store';

import { LanguageStateModel } from './language-state.model';
import { LanguageState } from './language.state';

export class LanguageSelectors {
  @Selector([LanguageState])
  static getLanguage(state: LanguageStateModel): string {
    return state.language;
  }
}
