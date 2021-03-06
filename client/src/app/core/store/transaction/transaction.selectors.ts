import { Selector } from '@ngxs/store';

import { TransactionState } from './transaction.state';
import { TransactionStateModel } from './transaction-state.model';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export class TransactionSelectors {
  @Selector([TransactionState])
  static transaction(state: TransactionStateModel): TransactionResponse {
    return state.transaction;
  }

  @Selector([TransactionState])
  static isLoading(state: TransactionStateModel): boolean {
    return state.isLoading;
  }

  @Selector([TransactionState])
  static error(state: TransactionStateModel): boolean {
    return state.error;
  }
}
