import { TokenInfo } from '@expensely/core/contracts';
import { Selector } from '@ngxs/store';

import { AuthenticationStateModel } from './authentication-state.model';
import { AuthenticationState } from './authentication.state';

export class AuthenticationSelectors {
  @Selector([AuthenticationState])
  static getIsLoading(state: AuthenticationStateModel): boolean {
    return state.isLoading;
  }

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

  @Selector([AuthenticationSelectors.getTokenInfo])
  static getUserId(tokenInfo: TokenInfo): string {
    return tokenInfo.userId;
  }

  @Selector([AuthenticationSelectors.getTokenInfo])
  static getUserPrimaryCurrency(tokenInfo: TokenInfo): number {
    return tokenInfo.primaryCurrency;
  }

  @Selector([AuthenticationSelectors.getTokenInfo])
  static getUserInitials(tokenInfo: TokenInfo): string {
    const nameParts = tokenInfo?.name.split(' ').map((part) => part.toUpperCase());

    if (nameParts?.length === 0 || nameParts?.some((x) => x.length === 0)) {
      return '';
    }

    return `${nameParts[0][0]}${nameParts[1][0]}`;
  }
}
