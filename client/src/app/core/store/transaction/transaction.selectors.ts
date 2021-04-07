import { Selector } from '@ngxs/store';

import { TransactionState } from './transaction.state';
import { TransactionStateModel } from './transaction-state.model';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export class TransactionSelectors {
  @Selector([TransactionState])
  static getTransactionId(state: TransactionStateModel): string {
    return state.transactionId;
  }

  @Selector([TransactionState])
  static getTransaction(state: TransactionStateModel): TransactionResponse {
    return state.transaction;
  }

  @Selector([TransactionState])
  static getIsLoading(state: TransactionStateModel): boolean {
    return state.isLoading;
  }

  @Selector([TransactionState])
  static getError(state: TransactionStateModel): boolean {
    return state.error;
  }
}
