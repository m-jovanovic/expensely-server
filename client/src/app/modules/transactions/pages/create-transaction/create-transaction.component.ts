import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import {
  TransactionFacade,
  CategoryFacade,
  CurrencyFacade,
  RouterService,
  TransactionType,
  CategoryResponse,
  CurrencyResponse,
  ApiErrorResponse
} from '@expensely/core';

@Component({
  selector: 'exp-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss']
})
export class CreateTransactionComponent implements OnInit {
  createTransactionForm: FormGroup;
  categories$: Observable<CategoryResponse[]>;
  currencies$: Observable<CurrencyResponse[]>;
  requestSent = false;

  constructor(
    private transactionFacade: TransactionFacade,
    private categoryFacade: CategoryFacade,
    private currencyFacade: CurrencyFacade,
    private formBuilder: FormBuilder,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.createTransactionForm = this.formBuilder.group({
      transactionType: [TransactionType.Expense.toString(), Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      occurredOn: [this.getCurrentDateString(), Validators.required]
    });

    this.categories$ = this.categoryFacade.categories$;

    this.currencies$ = this.currencyFacade.currencies$.pipe(
      tap((currenciesArray) => {
        if (!currenciesArray?.length) {
          return;
        }

        // TODO: Default this to primary currency when implemented in response.
        let firstCurrency = currenciesArray[0];

        this.createTransactionForm.controls.currency.setValue(firstCurrency.id);
      })
    );

    this.categoryFacade.loadCategories();

    this.currencyFacade.loadCurrencies();
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/dashboard');
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

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
        catchError((error: HttpErrorResponse) => {
          this.handleCreateTransactionError(new ApiErrorResponse(error));

          return of(true);
        }),
        tap(() => {
          this.requestSent = false;
          this.createTransactionForm.enable();
        })
      )
      .subscribe(() => this.routerService.navigateByUrl('/dashboard'));
  }

  private handleCreateTransactionError(errorResponse: ApiErrorResponse): void {}

  private getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
