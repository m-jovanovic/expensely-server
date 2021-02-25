import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { Login, Logout, RefreshToken, Register } from './authentication.actions';
import { AuthenticationSelectors } from './authentication.selectors';
import { JwtService } from '../../services/common/jwt-service';
import { TokenInfo } from '../../contracts/authentication/token-info';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationFacade {
  @Select(AuthenticationSelectors.getIsLoggedIn)
  isLoggedIn$: Observable<boolean>;

  constructor(private store: Store, private jwtService: JwtService) {}

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
    return this.store.selectSnapshot(AuthenticationSelectors.getToken);
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
}
