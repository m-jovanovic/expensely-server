import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, first } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ApiErrorResponse } from '../../../core/contracts/errors/api-error-response';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService {
  constructor(private client: HttpClient) {}

  protected get<T>(route: string): Observable<T> {
    return this.client.get<T>(`${environment.apiUrl}/${route}`).pipe(first(), catchError(this.handleHttpError));
  }

  protected post<T>(route: string, body?: any): Observable<T> {
    return this.client.post<T>(`${environment.apiUrl}/${route}`, body).pipe(first(), catchError(this.handleHttpError));
  }

  protected put<T>(route: string, body?: any): Observable<T> {
    return this.client.put<T>(`${environment.apiUrl}/${route}`, body).pipe(first(), catchError(this.handleHttpError));
  }

  protected delete(route: string): Observable<any> {
    return this.client.delete(`${environment.apiUrl}/${route}`).pipe(first(), catchError(this.handleHttpError));
  }

  private handleHttpError(httpErrorResponse: HttpErrorResponse): Observable<never> {
    const apiErrorResponse = new ApiErrorResponse(httpErrorResponse);

    return throwError(apiErrorResponse);
  }
}
