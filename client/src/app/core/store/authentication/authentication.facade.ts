import { Injectable } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Login, Logout, Register } from './authentication.actions';
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

  logout(): Observable<any> {
    return this.store.dispatch(new Logout());
  }

  register(firstName: string, lastName: string, email: string, password: string, confirmationPassword: string): Observable<any> {
    return this.store.dispatch(new Register(firstName, lastName, email, password, confirmationPassword));
  }

  get token(): string {
    return this.store.selectSnapshot(AuthenticationState.token);
  }
}
