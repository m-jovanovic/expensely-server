import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

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
import { finalize, tap } from 'rxjs/operators';

@Component({
  selector: 'exp-create-budget',
  templateUrl: './create-budget.component.html',
  styleUrls: ['./create-budget.component.scss']
})
export class CreateBudgetComponent implements OnInit {
  private requestSent = false;
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
    private dateService: DateService
  ) {}

  ngOnInit(): void {
    this.createBudgetForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      categories: [''],
      startDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.startDateBeforeEndDate]],
      endDate: [this.dateService.getCurrentDateString(), [Validators.required, DateRangeValidators.endDateAfterStartDate]]
    });

    this.currencies$ = this.userFacade.currencies$.pipe(
      tap((userCurrencies: UserCurrencyResponse[]) => {
        if (!userCurrencies?.length) {
          return;
        }

        this.createBudgetForm.controls.currency.setValue(userCurrencies.find((x) => x.isPrimary).id);
      })
    );

    this.categories$ = this.categoryFacade.expenseCategories$;

    this.isLoading$ = this.budgetFacade.isLoading$;

    this.userFacade.loadUserCurrencies();

    this.categoryFacade.loadCategories();
  }

  onSubmit(): void {
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
        this.createBudgetForm.value.categories || [],
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
        () => this.routerService.navigateByUrl(''),
        (error: ApiErrorResponse) => this.handleCreateBudgetError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('');
  }

  private handleCreateBudgetError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle errors.
    console.log('Failed to create budget.');
  }
}
