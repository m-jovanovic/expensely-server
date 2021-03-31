import { Injectable } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationFacade } from '../authentication/authentication.facade';
import { CreateBudget } from './budget.actions';
import { BudgetSelectors } from './budget.selectors';

@Injectable({
  providedIn: 'root'
})
export class BudgetFacade {
  @Select(BudgetSelectors.getIsLoading)
  isLoading$: Observable<boolean>;

  @Select(BudgetSelectors.getError)
  error$: Observable<boolean>;

  constructor(private store: Store, private authenticationFacade: AuthenticationFacade) {}

  createBudget(name: string, amount: number, currency: number, categories: number[], startDate: Date, endDate: Date): Observable<any> {
    return this.store.dispatch(new CreateBudget(this.authenticationFacade.userId, name, amount, currency, categories, startDate, endDate));
  }
}
