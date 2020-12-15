import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { State, StateContext, Action, Selector } from '@ngxs/store';

import { AuthenticationFacade } from '../authentication';
import { TransactionStateModel } from './transaction-state.model';
import { LoadTransactions } from './transaction.actions';
import { TransactionService } from '../../services/transaction/transaction.service';
import { TransactionListResponse, TransactionResponse } from '../../contracts/transaction';

@State<TransactionStateModel>({
  name: 'transactions',
  defaults: {
    transactions: [],
    cursor: '',
    isLoading: false,
    error: false
  }
})
@Injectable()
export class TransactionState {
  @Selector()
  static transactions(state: TransactionStateModel): TransactionResponse[] {
    return state.transactions;
  }

  @Selector()
  static isLoading(state: TransactionStateModel): boolean {
    return state.isLoading;
  }

  @Selector()
  static error(state: TransactionStateModel): boolean {
    return state.error;
  }

  constructor(private transactionService: TransactionService, private authenticationFacade: AuthenticationFacade) {}

  @Action(LoadTransactions)
  loadTransactions(context: StateContext<TransactionStateModel>, action: LoadTransactions): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.transactionService.getTransactions(action.userId, action.limit, context.getState().cursor).pipe(
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
}
