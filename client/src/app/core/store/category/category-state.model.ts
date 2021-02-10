import { CategoryResponse } from '../../contracts/transactions/category-response';

export interface CategoryStateModel {
  categories: CategoryResponse[];
  isLoading: boolean;
}
