import { Component, OnInit } from '@angular/core';
import { ExpensesPerCategoryFacade } from '@expensely/core';
import { TranslocoService } from '@ngneat/transloco';
import { ChartOptions, ChartType } from 'chart.js';
import { Label, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip, SingleDataSet } from 'ng2-charts';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'exp-expenses-per-category-chart',
  templateUrl: './expenses-per-category-chart.component.html',
  styleUrls: ['./expenses-per-category-chart.component.scss']
})
export class ExpensesPerCategoryChartComponent implements OnInit {
  public isLoading$: Observable<boolean>;
  public chartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: true,
    legend: {
      position: 'bottom'
    }
  };
  public chartLabels$: Observable<Label[]>;
  public chartData$: Observable<SingleDataSet>;
  public chartType: ChartType = 'pie';
  public chartLegend = true;

  constructor(private expensesPerCategoryFacade: ExpensesPerCategoryFacade, private translationService: TranslocoService) {}

  ngOnInit(): void {
    monkeyPatchChartJsLegend();

    monkeyPatchChartJsTooltip();

    this.isLoading$ = this.expensesPerCategoryFacade.isLoading$;

    const expensesPerCategory$ = this.expensesPerCategoryFacade.expensesPerCategory$.pipe(
      filter((expensesPerCategory) => expensesPerCategory?.length > 0)
    );

    this.chartLabels$ = expensesPerCategory$.pipe(
      map((expensesPerCategory) => expensesPerCategory.map((item) => this.translationService.translate(`categories.${item.category}`)))
    );

    this.chartData$ = expensesPerCategory$.pipe(map((expensesPerCategory) => expensesPerCategory.map((item) => item.amount)));

    this.expensesPerCategoryFacade.loadExpensesPerCategory();
  }
}
