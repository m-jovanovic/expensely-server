import { Selector } from '@ngxs/store';

import { CurrencyState } from './currency.state';
import { CurrencyStateModel } from './currency-state.model';
import { CategoryResponse } from '../../contracts';

export class CurrencySelectors {
  @Selector([CurrencyState])
  static getCurrencies(state: CurrencyStateModel): CategoryResponse[] {
    return state.currencies;
  }

  @Selector([CurrencyState])
  static getIsLoading(state: CurrencyStateModel): boolean {
    return state.isLoading;
  }
}
