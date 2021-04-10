import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { initialState, TransactionStateModel } from './transaction-state.model';
import { GetTransaction, DeleteTransaction, CreateTransaction, UpdateTransaction, GetTransactionDetails } from './transaction.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import {
  ApiErrorResponse,
  CreateTransactionRequest,
  EntityCreatedResponse,
  TransactionResponse,
  TransactionDetailsResponse,
  UpdateTransactionRequest
} from '../../contracts';

@State<TransactionStateModel>({
  name: 'transaction',
  defaults: initialState
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
          transactionId: response.id,
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

  @Action(GetTransactionDetails)
  getTransactionDetails(context: StateContext<TransactionStateModel>, action: GetTransactionDetails): Observable<any> {
    context.patchState({
      transactionDetails: null,
      isLoading: true
    });

    return this.transactionService.getTransactionDetails(action.transactionId).pipe(
      tap((response: TransactionDetailsResponse) => {
        context.patchState({
          transactionId: response.id,
          transactionDetails: response,
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
  createTransaction(context: StateContext<TransactionStateModel>, action: CreateTransaction): Observable<EntityCreatedResponse> {
    context.patchState({
      isLoading: true
    });

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
        tap((response: EntityCreatedResponse) => {
          context.patchState({
            transactionId: response.entityId,
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

  @Action(UpdateTransaction)
  updateTransaction(context: StateContext<TransactionStateModel>, action: UpdateTransaction): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService
      .updateTransaction(
        action.transactionId,
        new UpdateTransactionRequest(action.description, action.category, action.amount, action.currency, action.occurredOn)
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

  @Action(DeleteTransaction)
  deleteTransaction(context: StateContext<TransactionStateModel>, action: DeleteTransaction): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.deleteTransaction(action.transactionId).pipe(
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
