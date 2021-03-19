import { Selector } from '@ngxs/store';

import { TransactionSummaryState } from './transaction-summary.state';
import { TransactionSummaryStateModel } from './transaction-summary-state.model';

export class TransactionSummarySelectors {
  @Selector([TransactionSummaryState])
  static getExpense(state: TransactionSummaryStateModel): string {
    return state.expense;
  }

  @Selector([TransactionSummaryState])
  static getIncome(state: TransactionSummaryStateModel): string {
    return state.income;
  }

  @Selector([TransactionSummaryState])
  static getIsLoading(state: TransactionSummaryStateModel): boolean {
    return state.isLoading;
  }

  @Selector([TransactionSummaryState])
  static getError(state: TransactionSummaryStateModel): boolean {
    return state.error;
  }
}
