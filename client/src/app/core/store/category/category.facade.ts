import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { LoadCategories } from './category.actions';
import { CategorySelectors } from './category.selectors';
import { CategoryResponse } from '@expensely/core/contracts/transactions/category-response';

@Injectable({
  providedIn: 'root'
})
export class CategoryFacade {
  @Select(CategorySelectors.getCategories)
  categories$: Observable<CategoryResponse[]>;

  @Select(CategorySelectors.getExpenseCategories)
  expenseCategories$: Observable<CategoryResponse[]>;

  @Select(CategorySelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  constructor(private store: Store) {}

  loadCategories(): Observable<any> {
    return this.store.dispatch(new LoadCategories());
  }
}
