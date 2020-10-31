import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { State, StateContext, Action, Selector } from '@ngxs/store';

import { AuthenticationStateModel } from './authentication-state.model';
import { Login, Logout, Register } from './authentication.actions';
import { TokenResponse, LoginRequest, RegisterRequest } from '../../contracts';
import { AuthenticationService } from '../../services';

@State<AuthenticationStateModel>({
  name: 'authentication',
  defaults: {
    token: ''
  }
})
@Injectable()
export class AuthenticationState {
  @Selector()
  static token(state: AuthenticationStateModel): string {
    return state.token;
  }

  @Selector()
  static isLoggedIn(state: AuthenticationStateModel): boolean {
    return !!state.token;
  }

  constructor(private authenticationService: AuthenticationService) {}

  @Action(Login)
  login(context: StateContext<AuthenticationStateModel>, action: Login): Observable<TokenResponse> {
    return this.authenticationService.login(new LoginRequest(action.email, action.password)).pipe(
      tap((response: TokenResponse) => {
        context.patchState({
          token: response.token
        });
      })
    );
  }

  @Action(Logout)
  logout(context: StateContext<AuthenticationStateModel>): Observable<any> {
    return this.authenticationService.logout().pipe(
      tap(() => {
        context.patchState({
          token: ''
        });
      })
    );
  }

  @Action(Register)
  register(context: StateContext<AuthenticationStateModel>, action: Register): Observable<TokenResponse> {
    return this.authenticationService
      .register(new RegisterRequest(action.firstName, action.lastName, action.email, action.password, action.confirmationPassword))
      .pipe(
        tap((response: TokenResponse) => {
          context.patchState({
            token: response.token
          });
        })
      );
  }
}
