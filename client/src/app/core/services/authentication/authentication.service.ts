import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { RouterService } from '../common/router.service';
import { LoginRequest, RefreshTokenRequest, RegisterRequest, TokenResponse } from '../../contracts';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends ApiService {
  constructor(client: HttpClient, private routerService: RouterService, private route: ActivatedRoute) {
    super(client);
  }

  login(request: LoginRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.login, request).pipe(
      tap(async () => {
        const returnUrl: string = this.route.snapshot.queryParamMap.get('returnUrl') || '/dashboard';

        await this.routerService.navigateByUrl(returnUrl);
      })
    );
  }

  async logout(returnUrl?: string): Promise<boolean> {
    return await this.routerService.navigateToLogin(returnUrl);
  }

  register(request: RegisterRequest): Observable<any> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.register, request);
  }

  refreshToken(request: RefreshTokenRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>(ApiRoutes.Authentication.refreshToken, request);
  }
}
