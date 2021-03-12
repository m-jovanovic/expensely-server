import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { TransactionListStateModel } from './transaction-list-state.model';
import { CreateTransaction, LoadMoreTransactions, LoadTransactions } from './transaction-list.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { CreateTransactionRequest, TransactionListResponse } from '../../contracts/transactions';

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

  @Action(CreateTransaction)
  createTransaction(context: StateContext<TransactionListStateModel>, action: CreateTransaction): Observable<any> {
    context.patchState({
      isLoading: true
    });

    // TODO: See if this should eagerly update transactions collection, or issue a new GET request?
    return this.transactionService
      .createTransaction(
        new CreateTransactionRequest(
          action.userId,
          action.description,
          action.category,
          action.amount,
          action.currency,
          action.occurredOn,
          action.transactionType
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
            isLoading: false,
            error: true
          });

          return throwError(error);
        })
      );
  }

  // @Action(DeleteTransaction)
  // deleteTransaction(context: StateContext<TransactionListStateModel>, action: DeleteTransaction): Observable<any> {
  //   context.patchState({
  //     isLoading: true
  //   });

  //   let initialTransactions = context.getState().transactions;

  //   let filteredTransactions = initialTransactions.filter((x) => x.id !== action.transactionId);

  //   return this.transactionService.deleteTransaction(action.transactionId).pipe(
  //     tap(() => {
  //       context.patchState({
  //         isLoading: false,
  //         error: false,
  //         transactions: filteredTransactions
  //       });
  //     }),
  //     catchError((error: HttpErrorResponse) => {
  //       context.patchState({
  //         transactions: initialTransactions,
  //         isLoading: false,
  //         error: true
  //       });

  //       return throwError(error);
  //     })
  //   );
  // }

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
      catchError((error: HttpErrorResponse) => {
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
