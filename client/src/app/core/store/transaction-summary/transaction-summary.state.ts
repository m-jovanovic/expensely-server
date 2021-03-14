import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { TransactionSummaryStateModel } from './transaction-summary-state.model';
import { LoadTransactionSummary } from './transaction-summary.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { ApiErrorResponse, TransactionSummaryResponse } from '../../contracts';

@State<TransactionSummaryStateModel>({
  name: 'transaction_summary',
  defaults: {
    expense: '',
    income: '',
    isLoading: false,
    error: false
  }
})
@Injectable()
export class TransactionSummaryState {
  constructor(private transactionService: TransactionService) {}

  @Action(LoadTransactionSummary)
  loadTransactionSummary(
    context: StateContext<TransactionSummaryStateModel>,
    action: LoadTransactionSummary
  ): Observable<TransactionSummaryResponse> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.getCurrentMonthTransactionSummary(action.userId, action.currency).pipe(
      tap((response: TransactionSummaryResponse) => {
        context.patchState({
          expense: response.formattedExpense,
          income: response.formattedIncome,
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
