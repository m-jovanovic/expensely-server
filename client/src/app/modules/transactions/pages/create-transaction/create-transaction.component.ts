import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import {
  TransactionListFacade,
  CategoryFacade,
  RouterService,
  TransactionType,
  CategoryResponse,
  ApiErrorResponse,
  UserFacade,
  UserCurrencyResponse
} from '@expensely/core';

@Component({
  selector: 'exp-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss']
})
export class CreateTransactionComponent implements OnInit {
  createTransactionForm: FormGroup;
  categories$: Observable<CategoryResponse[]>;
  currencies$: Observable<UserCurrencyResponse[]>;
  requestSent = false;

  constructor(
    private transactionFacade: TransactionListFacade,
    private categoryFacade: CategoryFacade,
    private userFacade: UserFacade,
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

    this.currencies$ = this.userFacade.currencies$.pipe(
      tap((currenciesArray) => {
        if (!currenciesArray?.length) {
          return;
        }

        this.createTransactionForm.controls.currency.setValue(currenciesArray.find((x) => x.isPrimary).id);
      })
    );

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
      .subscribe(() => this.routerService.navigateByUrl('/transactions'));
  }

  private handleCreateTransactionError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle errors.
  }

  private getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
