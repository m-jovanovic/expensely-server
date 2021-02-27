import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { take } from 'rxjs/operators';
import { RouterService } from '../services';

import { AuthenticationFacade } from '../store/authentication/authentication.facade';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate, CanLoad {
  constructor(private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return this.isAuthenticated(state.url);
  }

  canLoad(route: Route, segments: UrlSegment[]): Promise<boolean> {
    return this.isAuthenticated(
      segments.map((urlSegment) => urlSegment.path).reduce((previous, current) => (previous += `/${current}`), '')
    );
  }

  private async isAuthenticated(returnUrl: string): Promise<boolean> {
    const isLoggedIn: boolean = await this.authenticationFacade.isLoggedIn$.pipe(take(1)).toPromise();

    if (isLoggedIn) {
      return true;
    }

    try {
      await this.authenticationFacade.refreshToken().toPromise();

      return true;
    } catch {
      await this.authenticationFacade.logout().toPromise();

      await this.routerService.navigateToLogin(returnUrl);

      return false;
    }
  }
}
