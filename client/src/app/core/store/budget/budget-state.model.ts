import { BudgetResponse } from '../../contracts/budgets/budget-response';

export interface BudgetStateModel {
  budgetId: string;
  budget: BudgetResponse;
  isLoading: boolean;
  error: boolean;
}

export const initialState: BudgetStateModel = {
  budgetId: '',
  budget: null,
  isLoading: false,
  error: false
};
