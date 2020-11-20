export interface AuthenticationStateModel {
  token: string;
  refreshToken: string;
  refreshTokenExpiresOnUtc: Date;
}
