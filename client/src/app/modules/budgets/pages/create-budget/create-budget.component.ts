import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { finalize, map, take, tap } from 'rxjs/operators';
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
  private selectedCategoriesSubject = new BehaviorSubject<CategoryResponse[]>([]);
  createBudgetForm: FormGroup;
  currencies$: Observable<UserCurrencyResponse[]>;
  categories$: Observable<CategoryResponse[]>;
  selectedCategories$: Observable<CategoryResponse[]>;
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
      category: [''],
      startDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.startDateBeforeEndDate]],
      endDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.endDateAfterStartDate]]
    });

    this.currencies$ = this.userFacade.currencies$.pipe(
      tap((userCurrencies: UserCurrencyResponse[]) => {
        if (!userCurrencies?.length) {
          return;
        }

        const primaryCurrency = userCurrencies.find((userCurrency) => userCurrency.isPrimary);

        this.createBudgetForm.controls.currency.setValue(primaryCurrency.id);
      })
    );

    this.selectedCategories$ = this.selectedCategoriesSubject.asObservable();

    this.categories$ = combineLatest([this.categoryFacade.expenseCategories$, this.selectedCategories$]).pipe(
      map(([categories, selectedCategories]) => {
        return categories.filter((category) => !selectedCategories.includes(category));
      })
    );

    this.isLoading$ = this.budgetFacade.isLoading$;

    this.userFacade.loadUserCurrencies();

    this.categoryFacade.loadCategories();
  }

  async onSubmit(): Promise<void> {
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
        await this.getSelectedCategoryIds(),
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

  addCategory(categoryId: number): void {
    this.categories$.pipe(take(1)).subscribe((categories: CategoryResponse[]) => {
      const category = categories.find((category) => category.id === categoryId);

      this.selectedCategories$.pipe(take(1)).subscribe((selectedCategories) => {
        this.selectedCategoriesSubject.next([...selectedCategories, category]);
      });
    });

    this.createBudgetForm.get('category').setValue('');
  }

  removeCategory(categoryToRemove: CategoryResponse): void {
    this.selectedCategories$.pipe(take(1)).subscribe((selectedCategories) => {
      this.selectedCategoriesSubject.next([...selectedCategories.filter((category) => category != categoryToRemove)]);
    });
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

  private async getSelectedCategoryIds(): Promise<number[]> {
    return await this.selectedCategories$
      .pipe(
        take(1),
        map((selectedCategories: CategoryResponse[]) => selectedCategories.map((c) => c.id))
      )
      .toPromise();
  }
}
