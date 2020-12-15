import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { IncomeStateModel } from './income-state.model';
import { CreateIncome, DeleteIncome, UpdateIncome } from './income.actions';
import { IncomeService } from '../../services/income/income.service';
import { CreateIncomeRequest, UpdateIncomeRequest } from '../../contracts/incomes';

@State<IncomeStateModel>({
  name: 'income',
  defaults: {
    isLoading: false,
    incomes: []
  }
})
@Injectable()
export class IncomeState {
  constructor(private incomeService: IncomeService, private authenticationFacade: AuthenticationFacade) {}

  @Action(CreateIncome)
  createExpense(context: StateContext<IncomeStateModel>, action: CreateIncome): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.incomeService
      .createIncome(
        new CreateIncomeRequest(
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
        }),
        catchError((error: HttpErrorResponse) => {
          context.patchState({
            isLoading: false
          });

          return throwError(error);
        })
      );
  }

  @Action(UpdateIncome)
  updateExpense(context: StateContext<IncomeStateModel>, action: UpdateIncome): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.incomeService
      .updateIncome(
        action.incomeId,
        new UpdateIncomeRequest(action.name, action.amount, action.currency, action.occurredOn, action.description)
      )
      .pipe(
        tap(() => {
          context.patchState({
            isLoading: false
          });
        }),
        catchError((error: HttpErrorResponse) => {
          context.patchState({
            isLoading: false
          });

          return throwError(error);
        })
      );
  }

  @Action(DeleteIncome)
  deleteExpense(context: StateContext<IncomeStateModel>, action: DeleteIncome): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.incomeService.deleteIncome(action.incomeId).pipe(
      tap(() => {
        context.patchState({
          isLoading: false
        });
      }),
      catchError((error: HttpErrorResponse) => {
        context.patchState({
          isLoading: false
        });

        return throwError(error);
      })
    );
  }
}
