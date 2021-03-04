import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { ChangePrimaryCurrency } from './user.actions';

@Injectable({
  providedIn: 'root'
})
export class UserFacade {
  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  changePrimaryCurrency(currency: number): Observable<any> {
    return this.store.dispatch(new ChangePrimaryCurrency(this.authenticationFacade.userId, currency));
  }
}
