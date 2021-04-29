import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { Label, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip, SingleDataSet } from 'ng2-charts';

@Component({
  selector: 'exp-expenses-per-category-chart',
  templateUrl: './expenses-per-category-chart.component.html',
  styleUrls: ['./expenses-per-category-chart.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExpensesPerCategoryChartComponent implements OnInit {
  chartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: true,
    legend: {
      position: 'bottom'
    },
    tooltips: {
      callbacks: {
        label: (item) => {
          return ` ${this.chartTooltips[item.index]}`;
        }
      }
    }
  };
  chartType: ChartType = 'pie';
  chartLegend = true;

  @Input()
  chartData: SingleDataSet;

  @Input()
  chartTooltips: string[];

  @Input()
  chartLabels: Label[];

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  constructor() {}

  ngOnInit(): void {
    monkeyPatchChartJsLegend();

    monkeyPatchChartJsTooltip();
  }
}
