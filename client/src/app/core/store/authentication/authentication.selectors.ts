import { TokenInfo } from '@expensely/core/contracts';
import { Selector } from '@ngxs/store';

import { AuthenticationStateModel } from './authentication-state.model';
import { AuthenticationState } from './authentication.state';

export class AuthenticationSelectors {
  @Selector([AuthenticationState])
  static getToken(state: AuthenticationStateModel): string {
    return state.token;
  }

  @Selector([AuthenticationState])
  static getTokenInfo(state: AuthenticationStateModel): TokenInfo {
    return state.tokenInfo;
  }

  @Selector([AuthenticationSelectors.getTokenInfo])
  static getIsLoggedIn(tokenInfo: TokenInfo): boolean {
    return tokenInfo?.exp > Date.now();
  }
}
