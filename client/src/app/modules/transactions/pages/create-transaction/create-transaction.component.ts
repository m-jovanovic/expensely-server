import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, Observable } from 'rxjs';
import { filter, finalize, map, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';

import {
  CategoryFacade,
  RouterService,
  TransactionType,
  CategoryResponse,
  ApiErrorResponse,
  UserFacade,
  UserCurrencyResponse,
  TransactionFacade,
  DateService
} from '@expensely/core';
import { NotificationService } from '@expensely/shared/services';
import { NotificationSettings } from '@expensely/shared/constants';

@Component({
  selector: 'exp-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss']
})
export class CreateTransactionComponent implements OnInit {
  private requestSent = false;
  createTransactionForm: FormGroup;
  categories$: Observable<CategoryResponse[]>;
  currencies$: Observable<UserCurrencyResponse[]>;
  isLoading$: Observable<boolean>;
  submitted = false;

  constructor(
    private transactionFacade: TransactionFacade,
    private categoryFacade: CategoryFacade,
    private userFacade: UserFacade,
    private formBuilder: FormBuilder,
    private routerService: RouterService,
    private dateService: DateService,
    private notificationService: NotificationService,
    private translationService: TranslocoService
  ) {}

  ngOnInit(): void {
    this.createTransactionForm = this.formBuilder.group({
      transactionType: [TransactionType.Expense.toString(), Validators.required],
      description: ['', [Validators.required, Validators.maxLength(100)]],
      category: ['', Validators.required],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      occurredOn: [this.dateService.getCurrentDateString(), Validators.required]
    });

    this.categories$ = combineLatest([this.categoryFacade.categories$, this.createTransactionForm.valueChanges]).pipe(
      map(([categories, formValue]) => {
        const isExpenseSelected = formValue.transactionType == TransactionType.Expense;

        return categories.filter((category) => category.isExpense == isExpenseSelected || category.isDefault);
      })
    );

    this.currencies$ = this.userFacade.currencies$.pipe(
      filter((userCurrencies: UserCurrencyResponse[]) => userCurrencies?.length > 0),
      tap((userCurrencies: UserCurrencyResponse[]) => {
        const primaryCurrency = userCurrencies.find((userCurrency) => userCurrency.isPrimary);

        this.createTransactionForm.controls.currency.setValue(primaryCurrency.id);
      })
    );

    this.isLoading$ = this.transactionFacade.isLoading$;

    this.categoryFacade.loadCategories();

    this.userFacade.loadUserCurrencies();
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;

    if (this.createTransactionForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.createTransactionForm.disable();

    const transactionType = this.createTransactionForm.value.transactionType;
    const sign = transactionType == TransactionType.Income ? 1 : -1;
    const amount = sign * this.createTransactionForm.value.amount;

    this.transactionFacade
      .createTransaction(
        this.createTransactionForm.value.description,
        this.createTransactionForm.value.category,
        amount,
        this.createTransactionForm.value.currency,
        this.createTransactionForm.value.occurredOn,
        transactionType
      )
      .pipe(
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.createTransactionForm.enable();
        })
      )
      .subscribe(
        () => this.routerService.navigate(['transactions', this.transactionFacade.transactionId]),
        (error: ApiErrorResponse) => this.handleCreateTransactionError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/transactions');
  }

  private handleCreateTransactionError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasErrors()) {
      this.notificationService.notify(
        this.translationService.translate('transactions.create.errors.serverError'),
        NotificationSettings.defaultTimeout
      );
    }
  }
}
