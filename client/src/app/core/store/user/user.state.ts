import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { State, StateContext, Action } from '@ngxs/store';

import { UserStateModel } from './user-state.mode';
import { AddCurrency, ChangePrimaryCurrency } from './user.actions';
import { UserService } from '../../../core/services';

@State<UserStateModel>({
  name: 'user'
})
@Injectable()
export class UserState {
  constructor(private userService: UserService) {}

  @Action(AddCurrency)
  addCurrency(context: StateContext<UserStateModel>, action: AddCurrency): Observable<any> {
    return this.userService.addCurrency(action.userId, action.currency);
  }

  @Action(ChangePrimaryCurrency)
  changePrimaryCurrency(context: StateContext<UserStateModel>, action: ChangePrimaryCurrency): Observable<any> {
    return this.userService.changePrimaryCurrency(action.userId, action.currency);
  }
}
