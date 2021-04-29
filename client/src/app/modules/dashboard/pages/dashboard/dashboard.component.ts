import { Component, OnInit } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Label, SingleDataSet } from 'ng2-charts';
import { combineLatest, Observable } from 'rxjs';

import {
  ExpensePerCategoryItem,
  ExpensesPerCategoryFacade,
  LanguageFacade,
  RouterService,
  TransactionListFacade,
  TransactionListItem,
  TransactionSummaryFacade
} from '@expensely/core';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private readonly numberOfTransactions = 10;
  transactionListTransactions$: Observable<TransactionListItem[]>;
  transactionListIsLoading$: Observable<boolean>;
  transactionListError$: Observable<boolean>;
  transactionSummaryExpense$: Observable<string>;
  transactionSummaryIncome$: Observable<string>;
  transactionSummaryIsLoading$: Observable<boolean>;
  transactionSummaryError$: Observable<boolean>;
  expensesPerCategoryChartData$: Observable<SingleDataSet>;
  expensesPerCategoryChartTooltips$: Observable<string[]>;
  expensesPerCategoryChartLabels$: Observable<Label[]>;
  expensesPerCategoryIsLoading$: Observable<boolean>;
  expensesPerCategoryError$: Observable<boolean>;

  constructor(
    private transactionListFacade: TransactionListFacade,
    private transactionSummaryFacade: TransactionSummaryFacade,
    private expensesPerCategoryFacade: ExpensesPerCategoryFacade,
    private languageFacade: LanguageFacade,
    private translationService: TranslocoService,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.transactionListTransactions$ = this.transactionListFacade.transactions$;
    this.transactionListIsLoading$ = this.transactionListFacade.isLoading$;
    this.transactionListError$ = this.transactionListFacade.error$;

    this.transactionSummaryExpense$ = this.transactionSummaryFacade.expense$;
    this.transactionSummaryIncome$ = this.transactionSummaryFacade.income$;
    this.transactionSummaryIsLoading$ = this.transactionSummaryFacade.isLoading$;
    this.transactionSummaryError$ = this.transactionSummaryFacade.error$;

    const expensesPerCategory$ = this.expensesPerCategoryFacade.expensesPerCategory$.pipe(filter((items) => items?.length > 0));
    this.expensesPerCategoryChartData$ = expensesPerCategory$.pipe(
      map((expensesPerCategory: ExpensePerCategoryItem[]) =>
        expensesPerCategory.map((expensePerCategory: ExpensePerCategoryItem) => expensePerCategory.amount)
      )
    );
    this.expensesPerCategoryChartTooltips$ = expensesPerCategory$.pipe(
      map((expensesPerCategory: ExpensePerCategoryItem[]) =>
        expensesPerCategory.map((expensePerCategory: ExpensePerCategoryItem) => expensePerCategory.formattedAmount)
      )
    );
    this.expensesPerCategoryChartLabels$ = combineLatest([expensesPerCategory$, this.languageFacade.currentLanguage$]).pipe(
      map(([expensesPerCategory, currentLanguage]) =>
        expensesPerCategory.map((item) => this.translationService.translate(`categories.${item.category}`, {}, currentLanguage))
      )
    );
    this.expensesPerCategoryIsLoading$ = this.expensesPerCategoryFacade.isLoading$;
    this.expensesPerCategoryError$ = this.expensesPerCategoryFacade.error$;

    this.transactionListFacade.loadTransactions(this.numberOfTransactions);
    this.transactionSummaryFacade.loadTransactionSummary();
    this.expensesPerCategoryFacade.loadExpensesPerCategory();
  }

  async selectTransaction(transactionId: string): Promise<boolean> {
    return await this.routerService.navigate(['/transactions', transactionId]);
  }
}
