import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { CurrencyResponse } from '../../contracts/transactions/currency-response';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getCurrencies(): Observable<CurrencyResponse[]> {
    return this.get(ApiRoutes.Currencies.getCurrencies);
  }
}
