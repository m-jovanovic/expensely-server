import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { BudgetStateModel } from './budget-state.model';
import { CreateBudget, GetBudget } from './budget.actions';
import { BudgetService } from '../../services/budgets/budget.service';
import { ApiErrorResponse, BudgetResponse, CreateBudgetRequest } from '../../contracts';

@State<BudgetStateModel>({
  name: 'budget',
  defaults: {
    budget: null,
    isLoading: false,
    error: false
  }
})
@Injectable()
export class BudgetState {
  constructor(private budgetService: BudgetService) {}

  @Action(GetBudget)
  getBudget(context: StateContext<BudgetStateModel>, action: GetBudget): Observable<any> {
    context.patchState({
      budget: null,
      isLoading: true
    });

    return this.budgetService.getBudget(action.budgetId).pipe(
      tap((response: BudgetResponse) => {
        context.patchState({
          budget: response,
          isLoading: false
        });
      }),
      catchError((error: ApiErrorResponse) => {
        context.patchState({
          isLoading: false,
          error: true
        });

        return throwError(error);
      })
    );
  }

  @Action(CreateBudget)
  createTransaction(context: StateContext<BudgetStateModel>, action: CreateBudget): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.budgetService
      .createBudget(
        new CreateBudgetRequest(
          action.userId,
          action.name,
          action.amount,
          action.currency,
          action.categories,
          action.startDate,
          action.endDate
        )
      )
      .pipe(
        tap(() => {
          context.patchState({
            isLoading: false
          });
        }),
        catchError((error: ApiErrorResponse) => {
          context.patchState({
            isLoading: false,
            error: true
          });

          return throwError(error);
        })
      );
  }
}
