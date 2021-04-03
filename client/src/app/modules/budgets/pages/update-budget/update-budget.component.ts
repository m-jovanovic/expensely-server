import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, Subscription } from 'rxjs';
import { finalize, map } from 'rxjs/operators';

import {
  ApiErrorResponse,
  BudgetFacade,
  BudgetResponse,
  CategoryFacade,
  CategoryResponse,
  DateRangeValidators,
  RouterService,
  UserCurrencyResponse,
  UserFacade
} from '@expensely/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'exp-update-budget',
  templateUrl: './update-budget.component.html',
  styleUrls: ['./update-budget.component.scss']
})
export class UpdateBudgetComponent implements OnInit, OnDestroy {
  private requestSent = false;
  private budgetSubscription: Subscription;
  updateBudgetForm: FormGroup;
  currencies$: Observable<UserCurrencyResponse[]>;
  categories$: Observable<CategoryResponse[]>;
  isLoading$: Observable<boolean>;
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private budgetFacade: BudgetFacade,
    private userFacade: UserFacade,
    private categoryFacade: CategoryFacade,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.updateBudgetForm = this.formBuilder.group({
      budgetId: '',
      name: ['', [Validators.required, Validators.maxLength(100)]],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      categories: [''],
      startDate: ['', [Validators.required, DateRangeValidators.startDateBeforeEndDate]],
      endDate: ['', [Validators.required, DateRangeValidators.endDateAfterStartDate]]
    });

    this.budgetSubscription = this.budgetFacade.budget$.subscribe((budget: BudgetResponse) => {
      if (!budget) {
        return;
      }

      this.updateBudgetForm.setValue({
        budgetId: budget.id,
        name: budget.name,
        amount: budget.amount,
        currency: budget.currency,
        categories: budget.categories.map((x) => x.id),
        startDate: budget.startDate.substring(0, 10),
        endDate: budget.endDate.substring(0, 10)
      });
    });

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

  ngOnDestroy(): void {
    this.budgetSubscription.unsubscribe();
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;

    if (this.updateBudgetForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.updateBudgetForm.disable();

    this.budgetFacade
      .updateBudget(
        this.updateBudgetForm.value.budgetId,
        this.updateBudgetForm.value.name,
        this.updateBudgetForm.value.amount,
        this.updateBudgetForm.value.currency,
        this.updateBudgetForm.value.startDate,
        this.updateBudgetForm.value.endDate
      )
      .pipe(
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.updateBudgetForm.enable();
        })
      )
      .subscribe(
        () => this.routerService.navigateByUrl(''),
        (error: ApiErrorResponse) => this.handleUpdateBudgetError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('');
  }

  private handleUpdateBudgetError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle errors.
    console.log('Failed to update budget.');
  }
}
