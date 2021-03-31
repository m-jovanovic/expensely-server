import { Selector } from '@ngxs/store';

import { BudgetStateModel } from './budget-state.model';
import { BudgetState } from './budget.state';

export class BudgetSelectors {
  @Selector([BudgetState])
  static getIsLoading(state: BudgetStateModel): boolean {
    return state.isLoading;
  }

  @Selector([BudgetState])
  static getError(state: BudgetStateModel): boolean {
    return state.error;
  }
}
