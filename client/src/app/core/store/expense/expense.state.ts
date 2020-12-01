import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/internal/operators/tap';
import { State, StateContext, Action } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { ExpenseStateModel } from './expense-state.model';
import { CreateExpense, DeleteExpense, UpdateExpense } from './expense.actions';
import { ExpenseService } from '../../services/expense/expense.service';
import { CreateExpenseRequest, UpdateExpenseRequest } from '../../contracts/expenses';

@State<ExpenseStateModel>({
  name: 'expense',
  defaults: {
    isLoading: false,
    expenses: []
  }
})
@Injectable()
export class ExpenseState {
  constructor(private expenseService: ExpenseService, private authenticationFacade: AuthenticationFacade) {}

  @Action(CreateExpense)
  createExpense(context: StateContext<ExpenseStateModel>, action: CreateExpense): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.expenseService
      .createExpense(
        new CreateExpenseRequest(
          this.authenticationFacade.userId,
          action.name,
          action.amount,
          action.currency,
          action.occurredOn,
          action.description
        )
      )
      .pipe(
        tap(() => {
          context.patchState({
            isLoading: false
          });
        })
      );
  }

  @Action(UpdateExpense)
  updateExpense(context: StateContext<ExpenseStateModel>, action: UpdateExpense): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.expenseService
      .updateExpense(
        action.expenseId,
        new UpdateExpenseRequest(action.name, action.amount, action.currency, action.occurredOn, action.description)
      )
      .pipe(
        tap(() => {
          context.patchState({
            isLoading: false
          });
        })
      );
  }

  @Action(DeleteExpense)
  deleteExpense(context: StateContext<ExpenseStateModel>, action: DeleteExpense): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.expenseService.deleteExpense(action.expenseId).pipe(
      tap(() => {
        context.patchState({
          isLoading: false
        });
      })
    );
  }
}
