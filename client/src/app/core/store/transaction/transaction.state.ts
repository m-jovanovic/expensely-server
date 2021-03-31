import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { TransactionStateModel } from './transaction-state.model';
import { GetTransaction, DeleteTransaction, CreateTransaction } from './transaction.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { ApiErrorResponse, CreateTransactionRequest, TransactionResponse } from '../../contracts';

@State<TransactionStateModel>({
  name: 'transaction',
  defaults: {
    transaction: null,
    isLoading: false,
    error: false
  }
})
@Injectable()
export class TransactionState {
  constructor(private transactionService: TransactionService) {}

  @Action(GetTransaction)
  getTransaction(context: StateContext<TransactionStateModel>, action: GetTransaction): Observable<any> {
    context.patchState({
      transaction: null,
      isLoading: true
    });

    return this.transactionService.getTransaction(action.transactionId).pipe(
      tap((response: TransactionResponse) => {
        context.patchState({
          transaction: response,
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

  @Action(CreateTransaction)
  createTransaction(context: StateContext<TransactionStateModel>, action: CreateTransaction): Observable<any> {
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
        catchError((error: ApiErrorResponse) => {
          context.patchState({
            isLoading: false,
            error: true
          });

          return throwError(error);
        })
      );
  }

  @Action(DeleteTransaction)
  deleteTransaction(context: StateContext<TransactionStateModel>, action: DeleteTransaction): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.deleteTransaction(action.transactionId).pipe(
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
