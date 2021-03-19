import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { take } from 'rxjs/operators';

import { RouterService } from '../services/common/router.service';
import { AuthorizationFacade } from '../store/authorization/authorization.facade';
import { AuthenticationFacade } from '../store/authentication/authentication.facade';
import { Permission } from '../contracts';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate, CanLoad {
  constructor(
    private authenticationFacade: AuthenticationFacade,
    private authorizationFacade: AuthorizationFacade,
    private routerService: RouterService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const permission: Permission = (route?.data?.permission as Permission) || null;

    const url = state.url;

    return this.isAuthenticated(url, permission);
  }

  canLoad(route: Route, segments: UrlSegment[]): Promise<boolean> {
    const permission: Permission = (route?.data?.permission as Permission) || null;

    const url = segments.map((urlSegment) => urlSegment.path).reduce((previous, current) => (previous += `/${current}`), '');

    return this.isAuthenticated(url, permission);
  }

  private async isAuthenticated(url: string, permission: Permission): Promise<boolean> {
    const isLoggedIn: boolean = await this.authenticationFacade.isLoggedIn$.pipe(take(1)).toPromise();

    const permissionIsNotNull = permission != null;

    const hasPermission = permissionIsNotNull && this.authorizationFacade.hasPermission(permission);

    if (isLoggedIn && permissionIsNotNull && !hasPermission) {
      return false;
    }

    if (isLoggedIn) {
      await this.navigateToSetupIfUserDidNotChoosePrimaryCurrency(url);

      return true;
    }

    try {
      await this.authenticationFacade.refreshToken().toPromise();

      return true;
    } catch {
      await this.authenticationFacade.logout().toPromise();

      await this.routerService.navigateToLogin(url);

      return false;
    }
  }

  private async navigateToSetupIfUserDidNotChoosePrimaryCurrency(url: string): Promise<boolean> {
    if (!url.includes('setup') && this.authenticationFacade.userPrimaryCurrency <= 0) {
      return await this.routerService.navigateByUrl('/setup');
    }

    return true;
  }
}
