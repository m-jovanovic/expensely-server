import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { CreateTransaction, LoadTransactions } from './transaction-list.actions';
import { TransactionListSelectors } from './transaction-list.selectors';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionListFacade {
  @Select(TransactionListSelectors.transactions)
  transactions$: Observable<TransactionResponse[]>;

  @Select(TransactionListSelectors.isLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionListSelectors.error)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  createTransaction(
    description: string,
    category: number,
    amount: number,
    currency: number,
    occurredOn: Date,
    transactionType: number
  ): Observable<any> {
    return this.store.dispatch(
      new CreateTransaction(this.authenticationFacade.userId, description, category, amount, currency, occurredOn, transactionType)
    );
  }

  loadTransactions(limit: number): Observable<any> {
    return this.store.dispatch(new LoadTransactions(this.authenticationFacade.userId, limit));
  }
}
