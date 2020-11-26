import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiService } from '../api/api.service';
import { TransactionSummaryResponse } from '@expensely/core/contracts';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getTransactionSummary(userId: string, primaryCurrency: number): Observable<TransactionSummaryResponse> {
    return this.get(`transactions/summary?userId=${userId}&primaryCurrency=${primaryCurrency}`);
  }
}
