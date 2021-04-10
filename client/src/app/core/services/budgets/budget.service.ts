import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiService } from '../api/api.service';
import { CreateBudgetRequest, BudgetResponse, UpdateBudgetRequest, EntityCreatedResponse, BudgetDetailsResponse } from '../../contracts';
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

  getBudgetDetails(budgetId: string): Observable<BudgetDetailsResponse> {
    return this.get(ApiRoutes.Budgets.getBudgetDetails.replace('{budgetId}', budgetId));
  }

  createBudget(request: CreateBudgetRequest): Observable<EntityCreatedResponse> {
    return this.post(ApiRoutes.Budgets.createBudget, request);
  }

  updateBudget(budgetId: string, request: UpdateBudgetRequest): Observable<any> {
    return this.put(ApiRoutes.Budgets.updateBudget.replace('{budgetId}', budgetId), request);
  }

  deleteBudget(budgetId: string): Observable<any> {
    return this.delete(ApiRoutes.Budgets.deleteBudget.replace('{budgetId}', budgetId));
  }
}
