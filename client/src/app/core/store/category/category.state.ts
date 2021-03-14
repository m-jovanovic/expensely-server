import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { CategoryStateModel } from './category-state.model';
import { LoadCategories } from './category.actions';
import { CategoryService } from '../../services/category/category.service';
import { ApiErrorResponse, CategoryResponse } from '../../contracts';

@State<CategoryStateModel>({
  name: 'categories',
  defaults: {
    categories: [],
    isLoading: false
  }
})
@Injectable()
export class CategoryState {
  constructor(private categoryService: CategoryService) {}

  @Action(LoadCategories)
  loadCategories(context: StateContext<CategoryStateModel>, action: LoadCategories): Observable<any> {
    context.patchState({
      isLoading: true
    });

    return this.categoryService.getCategories().pipe(
      tap((response: CategoryResponse[]) => {
        context.patchState({
          categories: response,
          isLoading: false
        });
      }),
      catchError((error: ApiErrorResponse) => {
        context.patchState({
          isLoading: false
        });

        return throwError(error);
      })
    );
  }
}
