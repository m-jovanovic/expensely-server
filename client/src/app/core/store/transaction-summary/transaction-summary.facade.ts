import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { LoadTransactionSummary } from './transaction-summary.actions';
import { TransactionSummaryState } from './transaction-summary.state';

@Injectable({
  providedIn: 'root'
})
export class TransactionSummaryFacade {
  @Select(TransactionSummaryState.income)
  income$: Observable<string>;

  @Select(TransactionSummaryState.expense)
  expense$: Observable<string>;

  @Select(TransactionSummaryState.isLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionSummaryState.error)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  loadTransactionSummary(): Observable<any> {
    return this.store.dispatch(
      new LoadTransactionSummary(this.authenticationFacade.tokenInfo?.userId, this.authenticationFacade.tokenInfo?.primaryCurrency)
    );
  }
}
