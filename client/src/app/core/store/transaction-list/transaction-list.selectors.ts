import { Selector } from '@ngxs/store';

import { TransactionListState } from './transaction-list.state';
import { TransactionListStateModel } from './transaction-list-state.model';
import { TransactionListItem } from '../../contracts/transactions/transaction-list-item';

export class TransactionListSelectors {
  @Selector([TransactionListState])
  static getTransactions(state: TransactionListStateModel): TransactionListItem[] {
    return state.transactions;
  }

  @Selector([TransactionListState])
  static getIsLoading(state: TransactionListStateModel): boolean {
    return state.isLoading;
  }

  @Selector([TransactionListState])
  static getError(state: TransactionListStateModel): boolean {
    return state.error;
  }
}
