import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';

import { CreateExpense, DeleteExpense, UpdateExpense } from './expense.actions';

@Injectable({
  providedIn: 'root'
})
export class ExpenseFacade {
  constructor(private store: Store) {}

  createExpense(name: string, amount: number, currency: number, occurredOn: Date, description?: string): Observable<any> {
    return this.store.dispatch(new CreateExpense(name, amount, currency, occurredOn, description));
  }

  updateExpense(
    expenseId: string,
    name: string,
    amount: number,
    currency: number,
    occurredOn: Date,
    description?: string
  ): Observable<any> {
    return this.store.dispatch(new UpdateExpense(expenseId, name, amount, currency, occurredOn, description));
  }

  deleteExpense(expenseId: string): Observable<any> {
    return this.store.dispatch(new DeleteExpense(expenseId));
  }
}
