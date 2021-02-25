import { Selector } from '@ngxs/store';

import { TransactionSummaryState } from './transaction-summary.state';
import { TransactionSummaryStateModel } from './transaction-summary-state.model';

export class TransactionSummarySelectors {
  @Selector([TransactionSummaryState])
  static expense(state: TransactionSummaryStateModel): string {
    return state.expense;
  }

  @Selector([TransactionSummaryState])
  static income(state: TransactionSummaryStateModel): string {
    return state.income;
  }

  @Selector([TransactionSummaryState])
  static isLoading(state: TransactionSummaryStateModel): boolean {
    return state.isLoading;
  }

  @Selector([TransactionSummaryState])
  static error(state: TransactionSummaryStateModel): boolean {
    return state.error;
  }
}
