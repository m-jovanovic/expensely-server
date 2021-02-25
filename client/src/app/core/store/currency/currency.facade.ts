import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { LoadCurrencies } from './currency.actions';
import { CurrencySelectors } from './currency.selectors';
import { CurrencyResponse } from '../../contracts/transactions/currency-response';

@Injectable({
  providedIn: 'root'
})
export class CurrencyFacade {
  @Select(CurrencySelectors.getCurrencies)
  currencies$: Observable<CurrencyResponse[]>;

  @Select(CurrencySelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  constructor(private store: Store) {}

  loadCurrencies(): Observable<any> {
    return this.store.dispatch(new LoadCurrencies());
  }
}
