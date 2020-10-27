import { Injectable } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Login, Logout } from './authentication.actions';
import { AuthenticationState } from './authentication.state';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationFacade {
  @Select(AuthenticationState.isLoggedIn)
  isLoggedIn$: Observable<boolean>;

  constructor(private store: Store) {}

  login(email: string, password: string): Observable<any> {
    return this.store.dispatch(new Login(email, password));
  }

  logout(): void {
    this.store.dispatch(new Logout());
  }

  get token(): string {
    return this.store.selectSnapshot(AuthenticationState.token);
  }
}
