import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService {
  constructor(private client: HttpClient) {}

  protected get<T>(route: string): Observable<T> {
    return this.client.get<T>(`${environment.apiUrl}/${route}`);
  }

  protected post<T>(route: string, body: any): Observable<T> {
    return this.client.post<T>(`${environment.apiUrl}/${route}`, body);
  }

  protected delete(route: string): Observable<any> {
    return this.client.delete(`${environment.apiUrl}/${route}`);
  }
}
