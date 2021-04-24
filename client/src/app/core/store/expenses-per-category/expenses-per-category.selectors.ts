import { Selector } from '@ngxs/store';

import { ExpensesPerCategoryState } from './expenses-per-category.state';
import { ExpensesPerCategoryStateModel } from './expenses-per-category-state.model';
import { ExpensePerCategoryItem } from '../../contracts/transactions/expense-per-category-item';

export class ExpensesPerCategorySelectors {
  @Selector([ExpensesPerCategoryState])
  static getExpensesPerCategory(state: ExpensesPerCategoryStateModel): ExpensePerCategoryItem[] {
    return state.expensesPerCategory;
  }

  @Selector([ExpensesPerCategoryState])
  static getIsLoading(state: ExpensesPerCategoryStateModel): boolean {
    return state.isLoading;
  }

  @Selector([ExpensesPerCategoryState])
  static getError(state: ExpensesPerCategoryStateModel): boolean {
    return state.error;
  }
}
