import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { LoadTransactions } from './transaction.actions';
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

  loadTransactions(limit: number): Observable<any> {
    return this.store.dispatch(new LoadTransactions(this.authenticationFacade.userId, limit));
  }
}
