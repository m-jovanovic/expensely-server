import { Selector } from '@ngxs/store';

import { TransactionListState } from './transaction-list.state';
import { TransactionListStateModel } from './transaction-list-state.model';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export class TransactionListSelectors {
  @Selector([TransactionListState])
  static getTransactions(state: TransactionListStateModel): TransactionResponse[] {
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
