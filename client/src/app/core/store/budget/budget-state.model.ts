import { BudgetResponse, BudgetDetailsResponse } from '../../contracts/budgets';

export interface BudgetStateModel {
  budgetId: string;
  budget: BudgetResponse;
  budgetDetails: BudgetDetailsResponse;
  isLoading: boolean;
  error: boolean;
}

export const initialState: BudgetStateModel = {
  budgetId: '',
  budget: null,
  budgetDetails: null,
  isLoading: false,
  error: false
};
