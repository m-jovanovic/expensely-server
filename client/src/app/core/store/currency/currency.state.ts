import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { CurrencyStateModel } from './currency-state.model';
import { LoadCurrencies } from './currency.actions';
import { CurrencyService } from '../../services/currency/currency.service';
import { ApiErrorResponse, CurrencyResponse } from '../../contracts';

@State<CurrencyStateModel>({
  name: 'currencies',
  defaults: {
    currencies: [],
    isLoading: false
  }
})
@Injectable()
export class CurrencyState {
  constructor(private currencyService: CurrencyService) {}

  @Action(LoadCurrencies)
  loadCurrencies(context: StateContext<CurrencyStateModel>, action: LoadCurrencies): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.currencyService.getCurrencies().pipe(
      tap((response: CurrencyResponse[]) => {
        context.patchState({
          currencies: response,
          isLoading: false
        });
      }),
      catchError((error: ApiErrorResponse) => {
        context.patchState({
          isLoading: false
        });

        return throwError(error);
      })
    );
  }
}
