import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { AuthenticationFacade } from '../store';
import { catchError, mergeMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RefreshTokenInterceptor implements HttpInterceptor {
  private readonly unauthorizedStatusCode = 401;

  constructor(private authenticationFacade: AuthenticationFacade) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status !== this.unauthorizedStatusCode) {
          return throwError(error);
        }

        return this.authenticationFacade.refreshToken().pipe(
          mergeMap(() => {
            return next.handle(req);
          }),
          catchError((error: HttpErrorResponse) => {
            return throwError(error);
          })
        );
      })
    );
  }
}
