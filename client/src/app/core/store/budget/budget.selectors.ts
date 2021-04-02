import { Selector } from '@ngxs/store';

import { BudgetStateModel } from './budget-state.model';
import { BudgetState } from './budget.state';
import { BudgetResponse } from '../../contracts/budgets/budget-response';

export class BudgetSelectors {
  @Selector([BudgetState])
  static getBudget(state: BudgetStateModel): BudgetResponse {
    return state.budget;
  }

  @Selector([BudgetState])
  static getIsLoading(state: BudgetStateModel): boolean {
    return state.isLoading;
  }

  @Selector([BudgetState])
  static getError(state: BudgetStateModel): boolean {
    return state.error;
  }
}
