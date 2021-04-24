import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import {
  CreateTransactionRequest,
  UpdateTransactionRequest,
  TransactionListResponse,
  TransactionResponse,
  TransactionDetailsResponse,
  TransactionSummaryResponse,
  EntityCreatedResponse,
  ExpensePerCategoryResponse
} from '../../contracts';

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

  getTransactionDetails(transactionId: string): Observable<TransactionDetailsResponse> {
    return this.get(ApiRoutes.Transactions.getTransactionDetails.replace('{transactionId}', transactionId));
  }

  getCurrentMonthTransactionSummary(userId: string, currency: number): Observable<TransactionSummaryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthTransactionSummary}?userId=${userId}&currency=${currency}`);
  }

  getCurrentMonthExpensesPerCategory(userId: string, currency: number): Observable<ExpensePerCategoryResponse> {
    return this.get(`${ApiRoutes.Transactions.getCurrentMonthExpensesPerCategory}?userId=${userId}&currency=${currency}`);
  }

  createTransaction(request: CreateTransactionRequest): Observable<EntityCreatedResponse> {
    return this.post(ApiRoutes.Transactions.createTransaction, request);
  }

  updateTransaction(transactionId: string, request: UpdateTransactionRequest): Observable<any> {
    return this.put(ApiRoutes.Transactions.updateTransaction.replace('{transactionId}', transactionId), request);
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.delete(ApiRoutes.Transactions.deleteTransaction.replace('{transactionId}', transactionId));
  }
}
