import { ExpenseResponse } from '../../contracts/expenses/expense-response';

export interface ExpenseStateModel {
  isLoading: boolean;
  expenses: ExpenseResponse[];
}
