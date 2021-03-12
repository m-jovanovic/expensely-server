import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { UserCurrencyResponse } from '../../contracts/users';

@Injectable({
  providedIn: 'root'
})
export class UserService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getUserCurrencies(userId: string): Observable<UserCurrencyResponse[]> {
    const url = ApiRoutes.Users.getUserCurrencies.replace('{userId}', userId);

    return this.get(url);
  }

  addUserCurrency(userId: string, currency: number): Observable<any> {
    const url = ApiRoutes.Users.addUserCurrency.replace('{userId}', userId).replace('{currency}', currency.toString());

    return this.post(url);
  }

  changeUserPrimaryCurrency(userId: string, currency: number): Observable<any> {
    const url = ApiRoutes.Users.changeUserPrimaryCurrency.replace('{userId}', userId).replace('{currency}', currency.toString());

    return this.put(url);
  }
}
