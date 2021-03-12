import { Selector } from '@ngxs/store';

import { UserState } from './user.state';
import { UserStateModel } from './user-state.model';
import { UserCurrencyResponse } from '../../contracts/users';

export class UserSelectors {
  @Selector([UserState])
  static getUserCurrencies(state: UserStateModel): UserCurrencyResponse[] {
    return state.currencies;
  }

  @Selector([UserState])
  static getIsLoading(state: UserStateModel): boolean {
    return state.isLoading;
  }
}
