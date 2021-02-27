import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { AuthenticationStateModel, initialState } from './authentication-state.model';
import { Login, Logout, RefreshToken, Register } from './authentication.actions';
import { TokenResponse, LoginRequest, RegisterRequest, RefreshTokenRequest } from '../../contracts';
import { AuthenticationService, JwtService } from '../../services';

@State<AuthenticationStateModel>({
  name: 'authentication',
  defaults: initialState
})
@Injectable()
export class AuthenticationState {
  constructor(private authenticationService: AuthenticationService, private jwtService: JwtService) {}

  @Action(Login)
  login(context: StateContext<AuthenticationStateModel>, action: Login): Observable<TokenResponse> {
    return this.authenticationService.login(new LoginRequest(action.email, action.password)).pipe(
      tap((response: TokenResponse) => {
        const authenticationState: AuthenticationStateModel = this.convertTokenResponseToAuthenticationStateModel(response);

        context.patchState(authenticationState);
      })
    );
  }

  @Action(Logout)
  logout(context: StateContext<AuthenticationStateModel>, action: Logout): void {
    context.patchState(initialState);

    this.authenticationService.logout();
  }

  @Action(Register)
  register(context: StateContext<AuthenticationStateModel>, action: Register): Observable<TokenResponse> {
    return this.authenticationService.register(
      new RegisterRequest(action.firstName, action.lastName, action.email, action.password, action.confirmationPassword)
    );
  }

  @Action(RefreshToken)
  refreshToken(context: StateContext<AuthenticationStateModel>): Observable<TokenResponse> {
    const state: AuthenticationStateModel = context.getState();

    if (!state?.refreshTokenExpiresOnUtc || new Date(state.refreshTokenExpiresOnUtc).getTime() < Date.now()) {
      return throwError(new Error('Refresh token has expired!'));
    }

    return this.authenticationService.refreshToken(new RefreshTokenRequest(state.refreshToken)).pipe(
      tap((response: TokenResponse) => {
        const authenticationState: AuthenticationStateModel = this.convertTokenResponseToAuthenticationStateModel(response);

        context.patchState(authenticationState);
      })
    );
  }

  private convertTokenResponseToAuthenticationStateModel(tokenResponse: TokenResponse): AuthenticationStateModel {
    return {
      token: tokenResponse.token,
      refreshToken: tokenResponse.refreshToken,
      refreshTokenExpiresOnUtc: tokenResponse.refreshTokenExpiresOnUtc,
      tokenInfo: this.jwtService.decodeToken(tokenResponse.token)
    };
  }
}
