import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { first, tap } from 'rxjs/operators';

import { ApiService } from '../api/api.service';
import { LoginRequest, RefreshTokenRequest, RegisterRequest, TokenResponse } from '../../contracts';
import { RouterService } from '../common/router.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends ApiService {
  constructor(client: HttpClient, private routerService: RouterService) {
    super(client);
  }

  login(request: LoginRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>('authentication/login', request).pipe(
      first(),
      tap((response: TokenResponse) => {
        if (response.token) {
          this.routerService.navigate(['']);
        }
      })
    );
  }

  logout(): Observable<any> {
    return this.routerService.navigate(['/login']);
  }

  register(request: RegisterRequest): Observable<any> {
    return this.post<TokenResponse>('authentication/register', request).pipe(first());
  }

  refreshToken(request: RefreshTokenRequest): Observable<TokenResponse> {
    return this.post<TokenResponse>('authentication/refresh-token', request).pipe(first());
  }
}
