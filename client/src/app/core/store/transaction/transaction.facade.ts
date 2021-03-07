import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { DeleteTransaction, GetTransaction } from './transaction.actions';
import { TransactionSelectors } from './transaction.selectors';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionFacade {
  @Select(TransactionSelectors.transaction)
  transaction$: Observable<TransactionResponse>;

  @Select(TransactionSelectors.isLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionSelectors.error)
  error$: Observable<boolean>;

  constructor(private store: Store) {}

  getTransaction(transactionId: string): Observable<any> {
    return this.store.dispatch(new GetTransaction(transactionId));
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.store.dispatch(new DeleteTransaction(transactionId));
  }
}
