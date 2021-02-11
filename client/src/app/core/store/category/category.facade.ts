import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';

import { LoadCategories } from './category.actions';
import { CategoryState } from './category.state';
import { CategoryResponse } from '@expensely/core/contracts/transactions/category-response';

@Injectable({
  providedIn: 'root'
})
export class CategoryFacade {
  @Select(CategoryState.categories)
  categories$: Observable<CategoryResponse[]>;

  @Select(CategoryState.isLoading)
  isLoading$: Observable<boolean>;

  constructor(private store: Store) {}

  loadCategories(): Observable<any> {
    return this.store.dispatch(new LoadCategories());
  }
}
