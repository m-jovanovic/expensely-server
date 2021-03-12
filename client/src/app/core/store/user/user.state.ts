import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { State, StateContext, Action } from '@ngxs/store';

import { initialState, UserStateModel } from './user-state.model';
import { LoadUserCurrencies, AddUserCurrency, ChangeUserPrimaryCurrency } from './user.actions';
import { UserService } from '../../../core/services';
import { catchError, tap } from 'rxjs/operators';
import { UserCurrencyResponse } from '@expensely/core/contracts';
import { HttpErrorResponse } from '@angular/common/http';

@State<UserStateModel>({
  name: 'user',
  defaults: initialState
})
@Injectable()
export class UserState {
  constructor(private userService: UserService) {}

  @Action(LoadUserCurrencies)
  loadUserCurrencies(context: StateContext<UserStateModel>, action: LoadUserCurrencies): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.userService.getUserCurrencies(action.userId).pipe(
      tap((response: UserCurrencyResponse[]) => {
        context.patchState({
          currencies: response,
          isLoading: false
        });
      }),
      catchError((error: HttpErrorResponse) => {
        context.patchState({
          isLoading: false,
          error: true
        });

        return throwError(error);
      })
    );
  }

  @Action(AddUserCurrency)
  addUserCurrency(context: StateContext<UserStateModel>, action: AddUserCurrency): Observable<any> {
    return this.userService.addUserCurrency(action.userId, action.currency);
  }

  @Action(ChangeUserPrimaryCurrency)
  changeUserPrimaryCurrency(context: StateContext<UserStateModel>, action: ChangeUserPrimaryCurrency): Observable<any> {
    return this.userService.changeUserPrimaryCurrency(action.userId, action.currency);
  }
}
