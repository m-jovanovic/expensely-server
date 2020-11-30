import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { CreateExpenseRequest, UpdateExpenseRequest } from '../../contracts/expenses';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  createExpense(request: CreateExpenseRequest): Observable<any> {
    return this.post(ApiRoutes.Expenses.createExpense, request);
  }

  updateExpense(expenseId: string, request: UpdateExpenseRequest): Observable<any> {
    return this.put(ApiRoutes.Expenses.updateExpense.replace('{expenseId}', expenseId), request);
  }

  deleteExpense(expenseId: string): Observable<any> {
    return this.delete(ApiRoutes.Expenses.deleteExpense.replace('{expenseId}', expenseId));
  }
}
