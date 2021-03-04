import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  changePrimaryCurrency(userId: string, currency: number): Observable<any> {
    const url = ApiRoutes.Users.changePrimaryCurrency.replace('{userId}', userId).replace('{currency}', currency.toString());

    return this.post(url);
  }
}
