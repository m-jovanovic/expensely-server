import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, Observable } from 'rxjs';
import { finalize, map, tap } from 'rxjs/operators';

import {
  CategoryFacade,
  RouterService,
  TransactionType,
  CategoryResponse,
  ApiErrorResponse,
  UserFacade,
  UserCurrencyResponse,
  TransactionFacade
} from '@expensely/core';

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
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.createTransactionForm = this.formBuilder.group({
      transactionType: [TransactionType.Expense.toString(), Validators.required],
      description: ['', [Validators.required, Validators.maxLength(100)]],
      category: ['', Validators.required],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      occurredOn: [this.getCurrentDateString(), Validators.required]
    });

    this.categories$ = combineLatest([this.categoryFacade.categories$, this.createTransactionForm.valueChanges]).pipe(
      map(([categories, formValue]) => {
        const isExpenseSelected = formValue.transactionType == TransactionType.Expense;

        return categories.filter((x) => x.isExpense == isExpenseSelected || x.isDefault);
      })
    );

    this.currencies$ = this.userFacade.currencies$.pipe(
      tap((userCurrencies: UserCurrencyResponse[]) => {
        if (!userCurrencies?.length) {
          return;
        }

        this.createTransactionForm.controls.currency.setValue(userCurrencies.find((x) => x.isPrimary).id);
      })
    );

    this.isLoading$ = this.transactionFacade.isLoading$;

    this.categoryFacade.loadCategories();

    this.userFacade.loadUserCurrencies();
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/transactions');
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
        Number.parseInt(this.createTransactionForm.value.category),
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
        () => this.routerService.navigateByUrl('/transactions'),
        (error: ApiErrorResponse) => this.handleCreateTransactionError(error)
      );
  }

  private handleCreateTransactionError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle errors.
    console.log('Failed to create transaction.');
  }

  private getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
