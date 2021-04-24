import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { ExpensesPerCategoryStateModel } from './expenses-per-category-state.model';
import { LoadExpensesPerCategory } from './expenses-per-category.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { ApiErrorResponse, ExpensePerCategoryResponse } from '../../contracts';

@State<ExpensesPerCategoryStateModel>({
  name: 'expenses_per_category',
  defaults: {
    expensesPerCategory: [],
    isLoading: false,
    error: false
  }
})
@Injectable()
export class ExpensesPerCategoryState {
  constructor(private transactionService: TransactionService) {}

  @Action(LoadExpensesPerCategory)
  loadExpensesPerCategory(
    context: StateContext<ExpensesPerCategoryStateModel>,
    action: LoadExpensesPerCategory
  ): Observable<ExpensePerCategoryResponse> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.getCurrentMonthExpensesPerCategory(action.userId, action.currency).pipe(
      tap((response: ExpensePerCategoryResponse) => {
        context.patchState({
          expensesPerCategory: response.items,
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
}
