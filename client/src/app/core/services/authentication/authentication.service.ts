import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { first, tap } from 'rxjs/operators';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { RouterService } from '../common/router.service';
import { JwtService } from '../common/jwt-service';
import { LoginRequest, RefreshTokenRequest, RegisterRequest, TokenResponse } from '../../contracts';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends ApiService {
  private refreshTokenTimer;

  constructor(client: HttpClient, private routerService: RouterService, private jwtService: JwtService, private route: ActivatedRoute) {
    super(client);
  }

  login(request: LoginRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.login, request).pipe(
      first(),
      tap((tokenResponse: TokenResponse) => {
        this.startRefreshTokenTimer(tokenResponse);

        const returnUrl: string = this.route.snapshot.queryParamMap.get('returnUrl') || '';

        this.routerService.navigate([returnUrl]);
      })
    );
  }

  logout(returnUrl?: string): Observable<any> {
    this.stopRefreshTokenTimer();

    const params: Params = returnUrl ? { queryParams: returnUrl } : null;

    return this.routerService.navigate(['/login'], params);
  }

  register(request: RegisterRequest): Observable<any> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.register, request).pipe(first());
  }

  refreshToken(request: RefreshTokenRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.refreshToken, request).pipe(
      first(),
      tap((tokenResponse: TokenResponse) => {
        this.startRefreshTokenTimer(tokenResponse);
      })
    );
  }

  private startRefreshTokenTimer(tokenResponse: TokenResponse): void {
    const tokenInfo = this.jwtService.decodeToken(tokenResponse.token);

    const tokenExpiresOn = new Date(tokenInfo.exp);

    const oneMinuteInMilliseconds = 60 * 1000;

    const tokenExpirationInMilliseconds = tokenExpiresOn.getTime() - Date.now();

    const timeoutInMilliseconds = tokenExpirationInMilliseconds - oneMinuteInMilliseconds;

    this.refreshTokenTimer = setTimeout(
      () => this.refreshToken(new RefreshTokenRequest(tokenResponse.refreshToken)).subscribe(),
      timeoutInMilliseconds
    );
  }

  private stopRefreshTokenTimer(): void {
    clearTimeout(this.refreshTokenTimer);
  }
}
