import { TokenInfo } from '../../contracts/authentication/token-info';

export interface AuthenticationStateModel {
  token: string;
  refreshToken: string;
  refreshTokenExpiresOnUtc: Date;
  tokenInfo: TokenInfo;
  isLoading: boolean;
}

export const initialState: AuthenticationStateModel = {
  token: '',
  refreshToken: '',
  refreshTokenExpiresOnUtc: null,
  tokenInfo: null,
  isLoading: false
};
