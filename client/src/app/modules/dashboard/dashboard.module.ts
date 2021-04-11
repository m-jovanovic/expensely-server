import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { TransactionListState, TransactionSummaryState } from '@expensely/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './pages';
import { RecentTransactionListComponent, TransactionSummaryComponent } from './components';

export const dashboardTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent, RecentTransactionListComponent],
  imports: [SharedModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionListState, TransactionSummaryState])],
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
