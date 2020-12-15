import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { TransactionListResponse } from '../../contracts/transaction/transaction-list-response';
import { TransactionSummaryResponse } from '../../contracts/transaction/transaction-summary-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getTransactions(userId: string, limit: number, cursor: string): Observable<TransactionListResponse> {
    return this.get(`${ApiRoutes.Transactions.getTransactions}?userId=${userId}&limit=${limit}&cursor=${cursor}`);
  }

  getCurrentMonthTransactionSummary(userId: string, primaryCurrency: number): Observable<TransactionSummaryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthTransactionSummary}?userId=${userId}&primaryCurrency=${primaryCurrency}`);
  }
}
