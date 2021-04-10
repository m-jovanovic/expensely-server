import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication/authentication.facade';
import { CreateBudget, DeleteBudget, GetBudget, GetBudgetDetails, UpdateBudget } from './budget.actions';
import { BudgetSelectors } from './budget.selectors';
import { BudgetResponse, BudgetDetailsResponse } from '../../contracts/budgets';

@Injectable({
  providedIn: 'root'
})
export class BudgetFacade {
  @Select(BudgetSelectors.getBudget)
  budget$: Observable<BudgetResponse>;

  @Select(BudgetSelectors.getBudgetDetails)
  budgetDetails$: Observable<BudgetDetailsResponse>;

  @Select(BudgetSelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  @Select(BudgetSelectors.getError)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  getBudget(budgetId: string): Observable<any> {
    return this.store.dispatch(new GetBudget(budgetId));
  }

  getBudgetDetails(budgetId: string): Observable<any> {
    return this.store.dispatch(new GetBudgetDetails(budgetId));
  }

  createBudget(name: string, amount: number, currency: number, categories: number[], startDate: Date, endDate: Date): Observable<any> {
    return this.store.dispatch(new CreateBudget(this.authenticationFacade.userId, name, amount, currency, categories, startDate, endDate));
  }

  updateBudget(
    budgetId: string,
    name: string,
    amount: number,
    currency: number,
    categories: number[],
    startDate: Date,
    endDate: Date
  ): Observable<any> {
    return this.store.dispatch(new UpdateBudget(budgetId, name, amount, currency, categories, startDate, endDate));
  }

  deleteBudget(budgetId: string): Observable<any> {
    return this.store.dispatch(new DeleteBudget(budgetId));
  }

  get budgetId(): string {
    return this.store.selectSnapshot(BudgetSelectors.getBudgetId);
  }
}
