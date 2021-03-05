import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { AddUserCurrency, ChangeUserPrimaryCurrency } from './user.actions';

@Injectable({
  providedIn: 'root'
})
export class UserFacade {
  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  addUserCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new AddUserCurrency(this.authenticationFacade.userId, currency));
  }

  changeUserPrimaryCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new ChangeUserPrimaryCurrency(this.authenticationFacade.userId, currency));
  }
}
