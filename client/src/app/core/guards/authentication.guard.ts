import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';

import { AuthenticationFacade } from '../store/authentication/authentication.facade';
import { RouterService } from '../services';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate, CanLoad {
  constructor(private facade: AuthenticationFacade, private routerService: RouterService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.isAuthenticated();
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
    return this.isAuthenticated();
  }

  private isAuthenticated(): Observable<boolean> {
    return this.facade.isLoggedIn$.pipe(
      take(1),
      tap((loggedIn: boolean) => {
        if (!loggedIn) {
          this.routerService.navigate(['/login']);
        }
      })
    );
  }
}
