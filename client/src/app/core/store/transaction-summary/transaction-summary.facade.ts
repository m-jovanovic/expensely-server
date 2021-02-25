import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { LoadTransactionSummary } from './transaction-summary.actions';
import { TransactionSummarySelectors } from './transaction-summary.selectors';

@Injectable({
  providedIn: 'root'
})
export class TransactionSummaryFacade {
  @Select(TransactionSummarySelectors.income)
  income$: Observable<string>;

  @Select(TransactionSummarySelectors.expense)
  expense$: Observable<string>;

  @Select(TransactionSummarySelectors.isLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionSummarySelectors.error)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  loadTransactionSummary(): Observable<any> {
    return this.store.dispatch(new LoadTransactionSummary(this.authenticationFacade.userId, this.authenticationFacade.userPrimaryCurrency));
  }
}
