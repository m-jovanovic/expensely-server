import { BudgetResponse } from '../../contracts/budgets/budget-response';

export interface BudgetStateModel {
  budget: BudgetResponse;
  isLoading: boolean;
  error: boolean;
}
