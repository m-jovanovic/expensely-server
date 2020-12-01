import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';

import { CreateIncome, DeleteIncome, UpdateIncome } from './income.actions';

@Injectable({
  providedIn: 'root'
})
export class IncomeFacade {
  constructor(private store: Store) {}

  createIncome(name: string, amount: number, currency: number, occurredOn: Date, description?: string): Observable<any> {
    return this.store.dispatch(new CreateIncome(name, amount, currency, occurredOn, description));
  }

  updateIncome(incomeId: string, name: string, amount: number, currency: number, occurredOn: Date, description?: string): Observable<any> {
    return this.store.dispatch(new UpdateIncome(incomeId, name, amount, currency, occurredOn, description));
  }

  deleteIncome(incomeId: string): Observable<any> {
    return this.store.dispatch(new DeleteIncome(incomeId));
  }
}
