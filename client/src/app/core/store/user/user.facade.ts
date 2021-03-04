import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { AddCurrency, ChangePrimaryCurrency } from './user.actions';

@Injectable({
  providedIn: 'root'
})
export class UserFacade {
  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  addCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new AddCurrency(this.authenticationFacade.userId, currency));
  }

  changePrimaryCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new ChangePrimaryCurrency(this.authenticationFacade.userId, currency));
  }
}
