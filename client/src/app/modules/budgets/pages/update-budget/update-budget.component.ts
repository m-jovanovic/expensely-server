import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BudgetFacade, BudgetResponse, CategoryFacade, CategoryResponse, UserCurrencyResponse, UserFacade } from '@expensely/core';

@Component({
  selector: 'exp-update-budget',
  templateUrl: './update-budget.component.html',
  styleUrls: ['./update-budget.component.scss']
})
export class UpdateBudgetComponent implements OnInit {
  private requestSent = false;
  budget$: Observable<BudgetResponse>;
  currencies$: Observable<UserCurrencyResponse[]>;
  categories$: Observable<CategoryResponse[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private route: ActivatedRoute,
    private budgetFacade: BudgetFacade,
    private userFacade: UserFacade,
    private categoryFacade: CategoryFacade
  ) {}

  ngOnInit(): void {
    this.budget$ = this.budgetFacade.budget$;
    this.categories$ = this.categoryFacade.expenseCategories$;
    this.currencies$ = this.userFacade.currencies$;
    this.isLoading$ = combineLatest([this.budgetFacade.isLoading$, this.userFacade.isLoading$, this.categoryFacade.isLoading$]).pipe(
      map(([budgetIsLoading, userIsLoading, categoryIsLoading]) => budgetIsLoading || userIsLoading || categoryIsLoading)
    );

    const budgetId = this.route.snapshot.paramMap.get('id');

    this.budgetFacade.getBudget(budgetId);
    this.userFacade.loadUserCurrencies();
    this.categoryFacade.loadCategories();
  }
}
