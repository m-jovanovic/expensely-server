import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { CreateTransaction, DeleteTransaction, GetTransaction } from './transaction.actions';
import { TransactionSelectors } from './transaction.selectors';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionFacade {
  @Select(TransactionSelectors.getTransaction)
  transaction$: Observable<TransactionResponse>;

  @Select(TransactionSelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  @Select(TransactionSelectors.getError)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  getTransaction(transactionId: string): Observable<any> {
    return this.store.dispatch(new GetTransaction(transactionId));
  }

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

  deleteTransaction(transactionId: string): Observable<any> {
    return this.store.dispatch(new DeleteTransaction(transactionId));
  }
}
