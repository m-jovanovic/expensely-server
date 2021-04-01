import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiService } from '../api/api.service';
import { CreateBudgetRequest } from '../../contracts/budgets/create-budget-request';
import { ApiRoutes } from '../../constants/api-routes';

@Injectable({
  providedIn: 'root'
})
export class BudgetService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  createBudget(request: CreateBudgetRequest): Observable<any> {
    return this.post(ApiRoutes.Budgets.createBudget, request);
  }
}
