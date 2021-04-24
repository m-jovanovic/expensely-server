import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication';
import { LoadExpensesPerCategory } from './expenses-per-category.actions';
import { ExpensesPerCategorySelectors } from './expenses-per-category.selectors';
import { ExpensePerCategoryItem } from '../../contracts/transactions/expense-per-category-item';

@Injectable({
  providedIn: 'root'
})
export class ExpensesPerCategoryFacade {
  @Select(ExpensesPerCategorySelectors.getExpensesPerCategory)
  expensesPerCategory$: Observable<ExpensePerCategoryItem[]>;

  @Select(ExpensesPerCategorySelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  @Select(ExpensesPerCategorySelectors.getError)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  loadExpensesPerCategory(): Observable<any> {
    return this.store.dispatch(
      new LoadExpensesPerCategory(this.authenticationFacade.userId, this.authenticationFacade.userPrimaryCurrency)
    );
  }
}
