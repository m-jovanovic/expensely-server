import { Selector } from '@ngxs/store';

import { BudgetStateModel } from './budget-state.model';
import { BudgetState } from './budget.state';
import { BudgetResponse, BudgetDetailsResponse } from '../../contracts/budgets';

export class BudgetSelectors {
  @Selector([BudgetState])
  static getBudgetId(state: BudgetStateModel): string {
    return state.budgetId;
  }

  @Selector([BudgetState])
  static getBudget(state: BudgetStateModel): BudgetResponse {
    return state.budget;
  }

  @Selector([BudgetState])
  static getBudgetDetails(state: BudgetStateModel): BudgetDetailsResponse {
    return state.budgetDetails;
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
