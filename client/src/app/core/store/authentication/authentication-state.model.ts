import { TokenInfo } from '../../contracts/authentication/token-info';

export interface AuthenticationStateModel {
  token: string;
  refreshToken: string;
  refreshTokenExpiresOnUtc: Date;
  tokenInfo: TokenInfo;
}

export const initialState: AuthenticationStateModel = {
  token: '',
  refreshToken: '',
  refreshTokenExpiresOnUtc: null,
  tokenInfo: null
};
