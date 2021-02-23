import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthenticationState } from './authentication.state';
import { Login, Logout, RefreshToken, Register } from './authentication.actions';
import { JwtService } from '../../services/common/jwt-service';
import { TokenInfo } from '../../contracts/authentication/token-info';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationFacade {
  isLoggedIn$: Observable<boolean>;

  constructor(private store: Store, private jwtService: JwtService) {
    this.initializeIsLoggedIn();
  }

  login(email: string, password: string): Observable<any> {
    return this.store.dispatch(new Login(email, password));
  }

  logout(returnUrl?: string): Observable<any> {
    return this.store.dispatch(new Logout(returnUrl));
  }

  register(firstName: string, lastName: string, email: string, password: string, confirmationPassword: string): Observable<any> {
    return this.store.dispatch(new Register(firstName, lastName, email, password, confirmationPassword));
  }

  refreshToken(): Observable<any> {
    return this.store.dispatch(new RefreshToken());
  }

  get token(): string | null {
    return this.store.selectSnapshot(AuthenticationState.token);
  }

  get tokenInfo(): TokenInfo | null {
    return this.decodeToken(this.token);
  }

  get userId(): string | null {
    return this.tokenInfo?.userId;
  }

  private decodeToken(token: string): TokenInfo | null {
    return this.jwtService.decodeToken(token);
  }

  private initializeIsLoggedIn(): void {
    this.isLoggedIn$ = this.store.select(AuthenticationState.token).pipe(map((token) => this.decodeToken(token)?.exp > Date.now()));
  }
}
