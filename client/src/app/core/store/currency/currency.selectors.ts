import { Selector } from '@ngxs/store';

import { CurrencyState } from './currency.state';
import { CurrencyStateModel } from './currency-state.model';
import { CurrencyResponse } from '../../contracts/transactions';

export class CurrencySelectors {
  @Selector([CurrencyState])
  static getCurrencies(state: CurrencyStateModel): CurrencyResponse[] {
    return state.currencies;
  }

  @Selector([CurrencyState])
  static getIsLoading(state: CurrencyStateModel): boolean {
    return state.isLoading;
  }
}
