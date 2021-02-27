import { Injectable, NgZone } from '@angular/core';
import { Params, Router, UrlSegment, UrlSegmentGroup, UrlTree } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  constructor(private zone: NgZone, private router: Router) {}

  async navigateByUrl(url: string | UrlTree): Promise<boolean> {
    return await this.zone.run(() => {
      return this.router.navigateByUrl(url);
    });
  }

  async navigate(paths: any[], queryParams?: Params | undefined): Promise<boolean> {
    return await this.zone.run(() => {
      return this.router.navigate(paths, { queryParams });
    });
  }

  async navigateToLogin(returnUrl?: string): Promise<boolean> {
    let loginUrl = '/login';

    if (returnUrl) {
      loginUrl += `?returnUrl=${returnUrl}`;
    }

    const urlTree = this.router.parseUrl(loginUrl);

    return await this.navigateByUrl(urlTree);
  }
}
