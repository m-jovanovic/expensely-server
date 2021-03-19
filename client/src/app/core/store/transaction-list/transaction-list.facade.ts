import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { LoadTransactions } from './transaction-list.actions';
import { TransactionListSelectors } from './transaction-list.selectors';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionListFacade {
  @Select(TransactionListSelectors.getTransactions)
  transactions$: Observable<TransactionResponse[]>;

  @Select(TransactionListSelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionListSelectors.getError)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  loadTransactions(limit: number): Observable<any> {
    return this.store.dispatch(new LoadTransactions(this.authenticationFacade.userId, limit));
  }
}
