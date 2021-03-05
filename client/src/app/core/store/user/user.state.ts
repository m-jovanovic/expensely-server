import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { State, StateContext, Action } from '@ngxs/store';

import { UserStateModel } from './user-state.mode';
import { AddUserCurrency, ChangeUserPrimaryCurrency } from './user.actions';
import { UserService } from '../../../core/services';

@State<UserStateModel>({
  name: 'user'
})
@Injectable()
export class UserState {
  constructor(private userService: UserService) {}

  @Action(AddUserCurrency)
  addCurrency(context: StateContext<UserStateModel>, action: AddUserCurrency): Observable<any> {
    return this.userService.addUserCurrency(action.userId, action.currency);
  }

  @Action(ChangeUserPrimaryCurrency)
  changePrimaryCurrency(context: StateContext<UserStateModel>, action: ChangeUserPrimaryCurrency): Observable<any> {
    return this.userService.changeUserPrimaryCurrency(action.userId, action.currency);
  }
}
