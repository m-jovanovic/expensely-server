import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { ApiErrorResponse } from '@expensely/core/contracts';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService {
  constructor(private client: HttpClient) {}

  protected get<T>(route: string): Observable<T> {
    return this.client.get<T>(`${environment.apiUrl}/${route}`).pipe(
      catchError((httpErrorResponse: HttpErrorResponse) => {
        const apiErrorResponse = new ApiErrorResponse(httpErrorResponse);

        return throwError(apiErrorResponse);
      })
    );
  }

  protected post<T>(route: string, body?: any): Observable<T> {
    return this.client.post<T>(`${environment.apiUrl}/${route}`, body).pipe(
      catchError((httpErrorResponse: HttpErrorResponse) => {
        const apiErrorResponse = new ApiErrorResponse(httpErrorResponse);

        return throwError(apiErrorResponse);
      })
    );
  }

  protected put<T>(route: string, body?: any): Observable<T> {
    return this.client.put<T>(`${environment.apiUrl}/${route}`, body).pipe(
      catchError((httpErrorResponse: HttpErrorResponse) => {
        const apiErrorResponse = new ApiErrorResponse(httpErrorResponse);

        return throwError(apiErrorResponse);
      })
    );
  }

  protected delete(route: string): Observable<any> {
    return this.client.delete(`${environment.apiUrl}/${route}`).pipe(
      catchError((httpErrorResponse: HttpErrorResponse) => {
        const apiErrorResponse = new ApiErrorResponse(httpErrorResponse);

        return throwError(apiErrorResponse);
      })
    );
  }
}
