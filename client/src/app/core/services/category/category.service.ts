import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from '../../constants/api-routes';
import { ApiService } from '../api/api.service';
import { CategoryResponse } from '../../contracts/transactions/category-response';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends ApiService {
  constructor(client: HttpClient) {
    super(client);
  }

  getCategories(): Observable<CategoryResponse[]> {
    return this.get(ApiRoutes.Categories.getCategories);
  }
}
