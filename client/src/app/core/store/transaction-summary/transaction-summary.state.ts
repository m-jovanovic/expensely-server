import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { State, StateContext, Action, Selector } from '@ngxs/store';

import { TransactionSummaryStateModel } from './transaction-summary-state.model';
import { LoadTransactionSummary } from './transaction-summary.actions';
import { TransactionService } from '@expensely/core/services/transaction/transaction.service';
import { TransactionSummaryResponse } from '@expensely/core/contracts/transaction/transaction-summary-response';

@State<TransactionSummaryStateModel>({
  name: 'transactionSummary',
  defaults: {
    expense: '',
    income: '',
    isLoading: false
  }
})
@Injectable()
export class TransactionSummaryState {
  @Selector()
  static isLoading(state: TransactionSummaryStateModel): boolean {
    return state.isLoading;
  }

  @Selector()
  static expense(state: TransactionSummaryStateModel): string {
    return state.expense;
  }

  @Selector()
  static income(state: TransactionSummaryStateModel): string {
    return state.income;
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

    return this.transactionService.getTransactionSummary(action.userId, action.primaryCurrency).pipe(
      tap((response: TransactionSummaryResponse) => {
        context.patchState({
          expense: response.formattedExpense,
          income: response.formattedIncome
        });
      }),
      tap(() => {
        context.patchState({
          isLoading: false
        });
      })
    );
  }
}