import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import {
  CreateTransactionRequest,
  TransactionListResponse,
  TransactionResponse,
  TransactionSummaryResponse
} from '../../../core/contracts/transactions';

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

  getTransaction(transactionId: string): Observable<TransactionResponse> {
    return this.get(ApiRoutes.Transactions.getTransaction.replace('{transactionId}', transactionId));
  }

  getCurrentMonthTransactionSummary(userId: string, currency: number): Observable<TransactionSummaryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthTransactionSummary}?userId=${userId}&currency=${currency}`);
  }

  createTransaction(request: CreateTransactionRequest): Observable<any> {
    return this.post(ApiRoutes.Transactions.createTransaction, request);
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.delete(ApiRoutes.Transactions.deleteTransaction.replace('{transactionId}', transactionId));
  }
}
