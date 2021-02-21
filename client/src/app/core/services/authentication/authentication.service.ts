import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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

  constructor(client: HttpClient, private routerService: RouterService, private jwtService: JwtService) {
    super(client);
  }

  login(request: LoginRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.login, request).pipe(
      first(),
      tap((tokenResponse: TokenResponse) => {
        this.startRefreshTokenTimer(tokenResponse);

        this.routerService.navigate(['']);
      })
    );
  }

  logout(): Observable<any> {
    this.stopRefreshTokenTimer();

    return this.routerService.navigate(['/login']);
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
