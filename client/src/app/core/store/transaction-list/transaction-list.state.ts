import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { TransactionListStateModel } from './transaction-list-state.model';
import { LoadMoreTransactions, LoadTransactions } from './transaction-list.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { ApiErrorResponse, TransactionListResponse } from '../../contracts';

@State<TransactionListStateModel>({
  name: 'transaction_list',
  defaults: {
    transactions: [],
    cursor: '',
    isLoading: false,
    error: false
  }
})
@Injectable()
export class TransactionListState {
  constructor(private transactionService: TransactionService) {}

  @Action(LoadTransactions)
  loadTransactions(context: StateContext<TransactionListStateModel>, action: LoadTransactions): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.getTransactions(action.userId, action.limit, '').pipe(
      tap((response: TransactionListResponse) => {
        context.patchState({
          transactions: response.items,
          cursor: response.cursor,
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

  @Action(LoadMoreTransactions)
  loadMoreTransactions(context: StateContext<TransactionListStateModel>, action: LoadMoreTransactions): Observable<any> {
    context.patchState({
      isLoading: true
    });

    const cursor = context.getState().cursor;

    if (cursor === '') {
      return;
    }

    let initialTransactions = context.getState().transactions;

    return this.transactionService.getTransactions(action.userId, action.limit, cursor).pipe(
      tap((response: TransactionListResponse) => {
        context.patchState({
          transactions: [...response.items, ...initialTransactions],
          cursor: response.cursor,
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
