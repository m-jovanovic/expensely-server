import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { BudgetStateModel, initialState } from './budget-state.model';
import { CreateBudget, DeleteBudget, GetBudget, GetBudgetDetails, UpdateBudget } from './budget.actions';
import { BudgetService } from '../../services/budgets/budget.service';
import {
  ApiErrorResponse,
  BudgetDetailsResponse,
  BudgetResponse,
  CreateBudgetRequest,
  EntityCreatedResponse,
  UpdateBudgetRequest
} from '../../contracts';

@State<BudgetStateModel>({
  name: 'budget',
  defaults: initialState
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
          budgetId: response.id,
          budget: response,
          isLoading: false,
          error: false
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

  @Action(GetBudgetDetails)
  getBudgetDetails(context: StateContext<BudgetStateModel>, action: GetBudgetDetails): Observable<any> {
    context.patchState({
      budgetDetails: null,
      isLoading: true
    });

    return this.budgetService.getBudgetDetails(action.budgetId).pipe(
      tap((response: BudgetDetailsResponse) => {
        context.patchState({
          budgetId: response.id,
          budgetDetails: response,
          isLoading: false,
          error: false
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
  createBudget(context: StateContext<BudgetStateModel>, action: CreateBudget): Observable<EntityCreatedResponse> {
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
        tap((response: EntityCreatedResponse) => {
          context.patchState({
            budgetId: response.entityId,
            isLoading: false,
            error: false
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

  @Action(UpdateBudget)
  updateBudget(context: StateContext<BudgetStateModel>, action: UpdateBudget): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.budgetService
      .updateBudget(
        action.budgetId,
        new UpdateBudgetRequest(action.name, action.amount, action.currency, action.categories, action.startDate, action.endDate)
      )
      .pipe(
        tap(() => {
          context.patchState({
            isLoading: false,
            error: false
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

  @Action(DeleteBudget)
  deleteBudget(context: StateContext<BudgetStateModel>, action: DeleteBudget): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.budgetService.deleteBudget(action.budgetId).pipe(
      tap(() => {
        context.patchState(initialState);
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
