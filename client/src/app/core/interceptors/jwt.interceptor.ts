import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';

import { AuthenticationFacade } from '../store';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JwtInterceptor implements HttpInterceptor {
  private readonly unauthorizedStatusCode = 401;
  private refreshTokenExecuting = false;
  private refreshTokenSubject = new BehaviorSubject<Object>(null);

  constructor(private authenticationFacade: AuthenticationFacade) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!req.url.startsWith(environment.apiUrl)) {
      return next.handle(req);
    }

    req = this.addAuthorizationHeader(req);

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status !== this.unauthorizedStatusCode) {
          return throwError(error);
        }

        return this.handle401Unauthorized(req, next);
      })
    );
  }

  private handle401Unauthorized(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.refreshTokenExecuting) {
      return this.refreshTokenSubject.pipe(
        filter((value) => value != null),
        take(1),
        switchMap(() => {
          return next.handle(this.addAuthorizationHeader(req));
        })
      );
    }

    this.refreshTokenExecuting = true;

    this.refreshTokenSubject.next(null);

    return this.authenticationFacade.refreshToken().pipe(
      switchMap(() => {
        this.refreshTokenExecuting = false;

        this.refreshTokenSubject.next(new Object());

        return next.handle(this.addAuthorizationHeader(req));
      })
    );
  }

  private addAuthorizationHeader(request: HttpRequest<any>): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.authenticationFacade.token}`
      }
    });
  }
}
