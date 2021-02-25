import { Selector } from '@ngxs/store';

import { AuthenticationStateModel } from './authentication-state.model';
import { AuthenticationState } from './authentication.state';

export class AuthenticationSelectors {
  @Selector([AuthenticationState])
  static getToken(state: AuthenticationStateModel): string {
    return state.token;
  }

  @Selector([AuthenticationState])
  static getIsLoggedIn(state: AuthenticationStateModel): boolean {
    return state.tokenInfo.exp > Date.now();
  }
}
