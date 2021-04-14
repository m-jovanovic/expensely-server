import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { filter, finalize, take, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';

import {
  ApiErrorResponse,
  BudgetFacade,
  CategoryFacade,
  CategoryResponse,
  DateRangeValidators,
  DateService,
  RouterService,
  UserCurrencyResponse,
  UserFacade
} from '@expensely/core';
import { NotificationService } from '@expensely/shared/services';
import { NotificationSettings } from '@expensely/shared/constants';

@Component({
  selector: 'exp-create-budget',
  templateUrl: './create-budget.component.html',
  styleUrls: ['./create-budget.component.scss']
})
export class CreateBudgetComponent implements OnInit {
  private requestSent = false;
  private selectedCategoryIds: number[] = [];
  createBudgetForm: FormGroup;
  currencies$: Observable<UserCurrencyResponse[]>;
  categories$: Observable<CategoryResponse[]>;
  isLoading$: Observable<boolean>;
  submitted = false;

  constructor(
    private budgetFacade: BudgetFacade,
    private userFacade: UserFacade,
    private categoryFacade: CategoryFacade,
    private formBuilder: FormBuilder,
    private routerService: RouterService,
    private dateService: DateService,
    private notificationService: NotificationService,
    private translationService: TranslocoService
  ) {}

  ngOnInit(): void {
    this.createBudgetForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      startDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.startDateBeforeEndDate]],
      endDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.endDateAfterStartDate]]
    });

    this.currencies$ = this.userFacade.currencies$.pipe(
      filter((userCurrencies: UserCurrencyResponse[]) => userCurrencies?.length > 0),
      tap((userCurrencies: UserCurrencyResponse[]) => {
        const primaryCurrency = userCurrencies.find((userCurrency) => userCurrency.isPrimary);

        this.createBudgetForm.controls.currency.setValue(primaryCurrency.id);
      })
    );

    this.categories$ = this.categoryFacade.expenseCategories$.pipe(
      filter((categories) => categories.length > 0),
      take(1)
    );

    this.isLoading$ = this.budgetFacade.isLoading$;

    this.userFacade.loadUserCurrencies();

    this.categoryFacade.loadCategories();
  }

  onSubmit(): Promise<void> {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;

    if (this.createBudgetForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.createBudgetForm.disable();

    this.budgetFacade
      .createBudget(
        this.createBudgetForm.value.name,
        this.createBudgetForm.value.amount,
        this.createBudgetForm.value.currency,
        this.selectedCategoryIds,
        this.createBudgetForm.value.startDate,
        this.createBudgetForm.value.endDate
      )
      .pipe(
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.createBudgetForm.enable();
        })
      )
      .subscribe(
        () => this.routerService.navigate(['budgets', this.budgetFacade.budgetId]),
        (error: ApiErrorResponse) => this.handleCreateBudgetError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/budgets');
  }

  setSelectedCategories(selectedCategories: number[]): void {
    this.selectedCategoryIds = selectedCategories;
  }

  private handleCreateBudgetError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle more specific errors when server-side functionality is implemented.
    if (errorResponse.hasErrors()) {
      this.notificationService.notify(
        this.translationService.translate('budgets.create.error.serverError'),
        NotificationSettings.defaultTimeout
      );
    }
  }
}
