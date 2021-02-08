import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { CreateTransaction, DeleteTransaction, LoadTransactions } from './transaction.actions';
import { TransactionState } from './transaction.state';
import { TransactionResponse } from '../../contracts/transaction/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionFacade {
  @Select(TransactionState.transactions)
  transactions$: Observable<TransactionResponse>;

  @Select(TransactionState.isLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionState.error)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  createTransaction(
    name: string,
    description: string,
    category: number,
    amount: number,
    currency: number,
    occurredOn: Date,
    transactionType: number
  ): Observable<any> {
    return this.store.dispatch(
      new CreateTransaction(this.authenticationFacade.userId, name, description, category, amount, currency, occurredOn, transactionType)
    );
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.store.dispatch(new DeleteTransaction(transactionId));
  }

  loadTransactions(limit: number): Observable<any> {
    return this.store.dispatch(new LoadTransactions(this.authenticationFacade.userId, limit));
  }
}
