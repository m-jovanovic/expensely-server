import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';
import { ChartsModule } from 'ng2-charts';

import { SharedModule } from '@expensely/shared';
import { ExpensesPerCategoryState, TransactionListState, TransactionSummaryState } from '@expensely/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './pages';
import { RecentTransactionListComponent, TransactionSummaryComponent } from './components';
import { ExpensesPerCategoryChartComponent } from './components/expenses-per-category-chart/expenses-per-category-chart.component';

export const dashboardTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent, RecentTransactionListComponent, ExpensesPerCategoryChartComponent],
  imports: [
    SharedModule,
    ChartsModule,
    DashboardRoutingModule,
    NgxsModule.forFeature([TransactionListState, TransactionSummaryState, ExpensesPerCategoryState])
  ],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'dashboard',
        loader: dashboardTranslationsLoader
      }
    }
  ]
})
export class DashboardModule {}
