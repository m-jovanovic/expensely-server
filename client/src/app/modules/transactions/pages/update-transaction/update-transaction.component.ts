import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, Subscription } from 'rxjs';
import { filter, finalize, map, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';

import {
  ApiErrorResponse,
  CategoryFacade,
  CategoryResponse,
  RouterService,
  TransactionFacade,
  TransactionResponse,
  TransactionType,
  UserCurrencyResponse,
  UserFacade
} from '@expensely/core';
import { NotificationService } from '@expensely/shared/services';
import { NotificationSettings } from '@expensely/shared/constants';

@Component({
  selector: 'exp-update-transaction',
  templateUrl: './update-transaction.component.html',
  styleUrls: ['./update-transaction.component.scss']
})
export class UpdateTransactionComponent implements OnInit {
  private requestSent = false;
  updateTransactionForm: FormGroup;
  transaction$: Observable<TransactionResponse>;
  categories$: Observable<CategoryResponse[]>;
  currencies$: Observable<UserCurrencyResponse[]>;
  isLoading$: Observable<boolean>;
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private transactionFacade: TransactionFacade,
    private userFacade: UserFacade,
    private categoryFacade: CategoryFacade,
    private routerService: RouterService,
    private notificationService: NotificationService,
    private translationService: TranslocoService
  ) {}

  ngOnInit(): void {
    this.updateTransactionForm = this.formBuilder.group({
      transactionId: '',
      transactionType: '',
      description: ['', [Validators.required, Validators.maxLength(100)]],
      category: ['', Validators.required],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      occurredOn: ['', Validators.required]
    });

    this.transaction$ = this.transactionFacade.transaction$.pipe(
      tap((transaction: TransactionResponse) => {
        if (!transaction) {
          return;
        }

        this.updateTransactionForm.setValue({
          transactionId: transaction.id,
          transactionType: transaction.transactionType,
          description: transaction.description,
          category: transaction.category,
          amount: Math.abs(transaction.amount).toFixed(2),
          currency: transaction.currency,
          occurredOn: transaction.occurredOn.substring(0, 10)
        });
      })
    );

    this.categories$ = combineLatest([this.categoryFacade.categories$, this.transactionFacade.transaction$]).pipe(
      filter(([, transaction]) => !!transaction),
      map(([categories, transaction]) => {
        const isExpense = transaction.transactionType == TransactionType.Expense;

        return categories.filter((category) => category.isExpense == isExpense || category.isDefault);
      })
    );

    this.currencies$ = this.userFacade.currencies$;

    this.isLoading$ = combineLatest([this.transactionFacade.isLoading$, this.userFacade.isLoading$, this.categoryFacade.isLoading$]).pipe(
      map(([budgetIsLoading, userIsLoading, categoryIsLoading]) => budgetIsLoading || userIsLoading || categoryIsLoading)
    );

    const transactionId = this.route.snapshot.paramMap.get('id');

    this.transactionFacade.getTransaction(transactionId);
    this.userFacade.loadUserCurrencies();
    this.categoryFacade.loadCategories();
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;

    if (this.updateTransactionForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.updateTransactionForm.disable();

    const transactionType = this.updateTransactionForm.value.transactionType;
    const sign = transactionType == TransactionType.Income ? 1 : -1;
    const amount = sign * this.updateTransactionForm.value.amount;

    this.transactionFacade
      .updateTransaction(
        this.updateTransactionForm.value.transactionId,
        this.updateTransactionForm.value.description,
        this.updateTransactionForm.value.category,
        amount,
        this.updateTransactionForm.value.currency,
        this.updateTransactionForm.value.occurredOn
      )
      .pipe(
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.updateTransactionForm.enable();
        })
      )
      .subscribe(
        () => this.routerService.navigate(['transactions', this.transactionFacade.transactionId]),
        (error: ApiErrorResponse) => this.handleUpdateTransactionError(error)
      );
  }

  async onCancel(): Promise<boolean> {
    return await this.routerService.navigate(['transactions', this.transactionFacade.transactionId]);
  }

  private handleUpdateTransactionError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasErrors()) {
      this.notificationService.notify(
        this.translationService.translate('transactions.update.errors.serverError'),
        NotificationSettings.defaultTimeout
      );
    }
  }
}
