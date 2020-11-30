import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { CreateIncomeRequest, UpdateIncomeRequest } from '../../contracts/incomes';

@Injectable({
  providedIn: 'root'
})
export class IncomeService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  createIncome(request: CreateIncomeRequest): Observable<any> {
    return this.post(ApiRoutes.Incomes.createIncome, request);
  }

  updateIncome(incomeId: string, request: UpdateIncomeRequest): Observable<any> {
    return this.put(ApiRoutes.Incomes.updateIncome.replace('{incomeId}', incomeId), request);
  }

  deleteIncome(incomeId: string): Observable<any> {
    return this.delete(ApiRoutes.Incomes.deleteIncome.replace('{incomeId}', incomeId));
  }
}
