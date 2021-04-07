import { BudgetResponse } from '../../contracts/budgets/budget-response';

export interface BudgetStateModel {
  budgetId: string;
  budget: BudgetResponse;
  isLoading: boolean;
  error: boolean;
}
