import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { UserSelectors } from './user.selectors';
import { AddUserCurrency, ChangeUserPrimaryCurrency, LoadUserCurrencies } from './user.actions';
import { UserCurrencyResponse } from '../../contracts/users';

@Injectable({
  providedIn: 'root'
})
export class UserFacade {
  @Select(UserSelectors.getUserCurrencies)
  currencies$: Observable<UserCurrencyResponse[]>;

  @Select(UserSelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  loadUserCurrencies(): Observable<any> {
    return this.store.dispatch(new LoadUserCurrencies(this.authenticationFacade.userId));
  }

  addUserCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new AddUserCurrency(this.authenticationFacade.userId, currency));
  }

  changeUserPrimaryCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new ChangeUserPrimaryCurrency(this.authenticationFacade.userId, currency));
  }
}
