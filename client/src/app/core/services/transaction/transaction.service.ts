import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { TransactionSummaryResponse } from '../../contracts/transaction/transaction-summary-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getCurrentMonthTransactionSummary(userId: string, primaryCurrency: number): Observable<TransactionSummaryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthTransactionSummary}?userId=${userId}&primaryCurrency=${primaryCurrency}`);
  }
}
