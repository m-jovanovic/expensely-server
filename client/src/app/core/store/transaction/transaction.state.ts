import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { TransactionStateModel } from './transaction-state.model';
import { GetTransaction, DeleteTransaction } from './transaction.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { TransactionResponse } from '../../contracts/transactions/transaction-response';

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
      catchError((error: HttpErrorResponse) => {
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
    return this.transactionService.deleteTransaction(action.transactionId).pipe(
      catchError((error: HttpErrorResponse) => {
        context.patchState({
          error: true
        });

        return throwError(error);
      })
    );
  }
}
