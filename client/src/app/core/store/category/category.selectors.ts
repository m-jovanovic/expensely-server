import { Selector } from '@ngxs/store';

import { CategoryState } from './category.state';
import { CategoryStateModel } from './category-state.model';
import { CategoryResponse } from '../../contracts';

export class CategorySelectors {
  @Selector([CategoryState])
  static getCategories(state: CategoryStateModel): CategoryResponse[] {
    return state.categories;
  }

  @Selector([CategoryState])
  static getIsLoading(state: CategoryStateModel): boolean {
    return state.isLoading;
  }
}
