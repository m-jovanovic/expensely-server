import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { LoadCurrencies } from './currency.actions';
import { CurrencyState } from './currency.state';
import { CurrencyResponse } from '../../contracts/transactions/currency-response';

@Injectable({
  providedIn: 'root'
})
export class CurrencyFacade {
  @Select(CurrencyState.currencies)
  transactions$: Observable<CurrencyResponse[]>;

  @Select(CurrencyState.isLoading)
  isLoading$: Observable<boolean>;

  constructor(private store: Store) {}

  loadCurrencies(): Observable<any> {
    return this.store.dispatch(new LoadCurrencies());
  }
}
