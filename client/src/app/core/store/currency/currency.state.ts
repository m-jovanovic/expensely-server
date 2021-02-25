import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { CurrencyStateModel } from './currency-state.model';
import { LoadCurrencies } from './currency.actions';
import { CurrencyService } from '../../services/currency/currency.service';
import { CurrencyResponse } from '../../contracts/transactions/currency-response';

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
      catchError((error: HttpErrorResponse) => {
        context.patchState({
          isLoading: false
        });

        return throwError(error);
      })
    );
  }
}
