import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';

import { AuthenticationFacade } from '../store/authentication/authentication.facade';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate, CanLoad {
  constructor(private authenticationFacade: AuthenticationFacade) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.isAuthenticated(state.url);
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
    return this.isAuthenticated(
      segments.map((urlSegment) => urlSegment.path).reduce((previous, current) => (previous += `/${current}`), '')
    );
  }

  private isAuthenticated(returnUrl: string): Observable<boolean> {
    return this.authenticationFacade.isLoggedIn$.pipe(
      take(1),
      tap((isLoggedIn: boolean) => {
        if (isLoggedIn) {
          return;
        }

        this.authenticationFacade.logout(returnUrl).subscribe();
      })
    );
  }
}
