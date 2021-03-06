import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action } from '@ngxs/store';

import { TransactionStateModel } from './transaction-state.model';
import { GetTransactionById } from './transaction.actions';
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

  @Action(GetTransactionById)
  loadTransactions(context: StateContext<TransactionStateModel>, action: GetTransactionById): Observable<any> {
    context.patchState({
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
}
