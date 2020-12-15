import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action, Selector } from '@ngxs/store';

import { TransactionSummaryStateModel } from './transaction-summary-state.model';
import { LoadTransactionSummary } from './transaction-summary.actions';
import { TransactionService } from '@expensely/core/services/transaction/transaction.service';
import { TransactionSummaryResponse } from '@expensely/core/contracts/transaction/transaction-summary-response';
import { HttpErrorResponse } from '@angular/common/http';

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
  @Selector()
  static expense(state: TransactionSummaryStateModel): string {
    return state.expense;
  }

  @Selector()
  static income(state: TransactionSummaryStateModel): string {
    return state.income;
  }

  @Selector()
  static isLoading(state: TransactionSummaryStateModel): boolean {
    return state.isLoading;
  }

  @Selector()
  static error(state: TransactionSummaryStateModel): boolean {
    return state.error;
  }

  constructor(private transactionService: TransactionService) {}

  @Action(LoadTransactionSummary)
  loadTransactionSummary(
    context: StateContext<TransactionSummaryStateModel>,
    action: LoadTransactionSummary
  ): Observable<TransactionSummaryResponse> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.getCurrentMonthTransactionSummary(action.userId, action.primaryCurrency).pipe(
      tap((response: TransactionSummaryResponse) => {
        context.patchState({
          expense: response.formattedExpense,
          income: response.formattedIncome,
          isLoading: false,
          error: false
        });
      }),
      catchError((error: HttpErrorResponse) => {
        context.patchState({
          isLoading: false,
          error: true
        });

        return throwError(error);
      })
    );
  }
}
