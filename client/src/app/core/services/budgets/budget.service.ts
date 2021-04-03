import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiService } from '../api/api.service';
import { CreateBudgetRequest, BudgetResponse, UpdateBudgetRequest } from '../../contracts/budgets';
import { ApiRoutes } from '../../constants/api-routes';

@Injectable({
  providedIn: 'root'
})
export class BudgetService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getBudget(budgetId: string): Observable<BudgetResponse> {
    return this.get(ApiRoutes.Budgets.getBudget.replace('{budgetId}', budgetId));
  }

  createBudget(request: CreateBudgetRequest): Observable<any> {
    return this.post(ApiRoutes.Budgets.createBudget, request);
  }

  updateBudget(budgetId: string, request: UpdateBudgetRequest): Observable<any> {
    return this.put(ApiRoutes.Budgets.updateBudget.replace('{budgetId}', budgetId), request);
  }
}
