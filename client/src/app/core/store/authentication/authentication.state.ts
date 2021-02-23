import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { State, StateContext, Action, Selector } from '@ngxs/store';

import { AuthenticationStateModel } from './authentication-state.model';
import { Login, Logout, RefreshToken, Register } from './authentication.actions';
import { TokenResponse, LoginRequest, RegisterRequest, RefreshTokenRequest } from '../../contracts';
import { AuthenticationService, RouterService } from '../../services';

@State<AuthenticationStateModel>({
  name: 'authentication',
  defaults: {
    token: '',
    refreshToken: '',
    refreshTokenExpiresOnUtc: null
  }
})
@Injectable()
export class AuthenticationState {
  @Selector()
  static token(state: AuthenticationStateModel): string {
    return state.token;
  }

  constructor(private authenticationService: AuthenticationService) {}

  @Action(Login)
  login(context: StateContext<AuthenticationStateModel>, action: Login): Observable<TokenResponse> {
    return this.authenticationService.login(new LoginRequest(action.email, action.password)).pipe(
      tap((response: TokenResponse) => {
        context.patchState({
          token: response.token,
          refreshToken: response.refreshToken,
          refreshTokenExpiresOnUtc: response.refreshTokenExpiresOnUtc
        });
      })
    );
  }

  @Action(Logout)
  logout(context: StateContext<AuthenticationStateModel>, action: Logout): Observable<any> {
    return this.authenticationService.logout(action.returnUrl);
  }

  @Action(Register)
  register(context: StateContext<AuthenticationStateModel>, action: Register): Observable<TokenResponse> {
    return this.authenticationService.register(
      new RegisterRequest(action.firstName, action.lastName, action.email, action.password, action.confirmationPassword)
    );
  }

  @Action(RefreshToken)
  refreshToken(context: StateContext<AuthenticationStateModel>): Observable<TokenResponse> {
    const authenticationState = context.getState();

    if (!authenticationState?.refreshTokenExpiresOnUtc || new Date(authenticationState.refreshTokenExpiresOnUtc).getTime() < Date.now()) {
      context.dispatch(new Logout());

      return of(null);
    }

    return this.authenticationService.refreshToken(new RefreshTokenRequest(authenticationState.refreshToken)).pipe(
      tap((response: TokenResponse) => {
        context.patchState({
          token: response.token,
          refreshToken: response.refreshToken,
          refreshTokenExpiresOnUtc: response.refreshTokenExpiresOnUtc
        });
      })
    );
  }
}
