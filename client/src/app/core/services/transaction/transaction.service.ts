import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { CreateTransactionRequest, TransactionListResponse, TransactionSummaryResponse } from '../../../core/contracts/transactions';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  createTransaction(request: CreateTransactionRequest): Observable<any> {
    return this.post(ApiRoutes.Transactions.createTransaction, request);
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.delete(ApiRoutes.Transactions.deleteTransaction.replace('{transactionId}', transactionId));
  }

  getTransactions(userId: string, limit: number, cursor: string): Observable<TransactionListResponse> {
    return this.get(`${ApiRoutes.Transactions.getTransactions}?userId=${userId}&limit=${limit}&cursor=${cursor}`);
  }

  getCurrentMonthTransactionSummary(userId: string, primaryCurrency: number): Observable<TransactionSummaryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthTransactionSummary}?userId=${userId}&primaryCurrency=${primaryCurrency}`);
  }
}
