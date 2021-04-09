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
    return this.get(ApiRoutes.Users.getUserCurrencies.replace('{userId}', userId));
  }

  addUserCurrency(userId: string, currency: number): Observable<any> {
    return this.put(ApiRoutes.Users.addUserCurrency.replace('{userId}', userId).replace('{currency}', currency.toString()));
  }

  changeUserPrimaryCurrency(userId: string, currency: number): Observable<any> {
    return this.put(ApiRoutes.Users.changeUserPrimaryCurrency.replace('{userId}', userId).replace('{currency}', currency.toString()));
  }
}
