import { ExpensePerCategoryItem } from '../../contracts/transactions/expense-per-category-item';

export interface ExpensesPerCategoryStateModel {
  expensesPerCategory: ExpensePerCategoryItem[];
  isLoading: boolean;
  error: boolean;
}
