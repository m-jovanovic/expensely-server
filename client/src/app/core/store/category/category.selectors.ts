import { Selector } from '@ngxs/store';

import { CategoryState } from './category.state';
import { CategoryStateModel } from './category-state.model';
import { CategoryResponse } from '../../contracts/transactions';

export class CategorySelectors {
  @Selector([CategoryState])
  static getCategories(state: CategoryStateModel): CategoryResponse[] {
    return state.categories;
  }

  @Selector([CategorySelectors.getCategories])
  static getExpenseCategories(categories: CategoryResponse[]): CategoryResponse[] {
    return categories.filter((x) => x.isExpense);
  }

  @Selector([CategoryState])
  static getIsLoading(state: CategoryStateModel): boolean {
    return state.isLoading;
  }
}
