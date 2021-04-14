import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, Observable } from 'rxjs';
import { filter, finalize, map, take, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';

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
import { NotificationService } from '@expensely/shared/services';
import { NotificationSettings } from '@expensely/shared/constants';

@Component({
  selector: 'exp-update-budget',
  templateUrl: './update-budget.component.html',
  styleUrls: ['./update-budget.component.scss']
})
export class UpdateBudgetComponent implements OnInit {
  private requestSent = false;
  private selectedCategoryIds: number[] = [];
  budget$: Observable<BudgetResponse>;
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
    private routerService: RouterService,
    private notificationService: NotificationService,
    private translationService: TranslocoService
  ) {}

  ngOnInit(): void {
    this.updateBudgetForm = this.formBuilder.group({
      budgetId: '',
      name: ['', [Validators.required, Validators.maxLength(100)]],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      startDate: ['', [Validators.required, DateRangeValidators.startDateBeforeEndDate]],
      endDate: ['', [Validators.required, DateRangeValidators.endDateAfterStartDate]]
    });

    this.budget$ = this.budgetFacade.budget$.pipe(
      filter((budget: BudgetResponse) => !!budget),
      tap((budget: BudgetResponse) => {
        this.updateBudgetForm.setValue({
          budgetId: budget.id,
          name: budget.name,
          amount: budget.amount,
          currency: budget.currency,
          startDate: budget.startDate.substring(0, 10),
          endDate: budget.endDate.substring(0, 10)
        });
      })
    );

    this.categories$ = this.categoryFacade.expenseCategories$.pipe(
      filter((categories) => categories.length > 0),
      take(1)
    );

    this.currencies$ = this.userFacade.currencies$;

    this.isLoading$ = combineLatest([this.budgetFacade.isLoading$, this.userFacade.isLoading$, this.categoryFacade.isLoading$]).pipe(
      map(([budgetIsLoading, userIsLoading, categoryIsLoading]) => budgetIsLoading || userIsLoading || categoryIsLoading)
    );

    const budgetId = this.route.snapshot.paramMap.get('id');

    this.budgetFacade.getBudget(budgetId);
    this.userFacade.loadUserCurrencies();
    this.categoryFacade.loadCategories();
  }

  onSubmit(): Promise<void> {
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
        this.selectedCategoryIds,
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
        () => this.routerService.navigate(['budgets', this.budgetFacade.budgetId]),
        (error: ApiErrorResponse) => this.handleUpdateBudgetError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigate(['budgets', this.budgetFacade.budgetId]);
  }

  setSelectedCategories(selectedCategories: number[]): void {
    this.selectedCategoryIds = selectedCategories;
  }

  private handleUpdateBudgetError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle more specific errors when server-side functionality is implemented.
    if (errorResponse.hasErrors()) {
      this.notificationService.notify(
        this.translationService.translate('budgets.update.error.serverError'),
        NotificationSettings.defaultTimeout
      );
    }
  }
}
