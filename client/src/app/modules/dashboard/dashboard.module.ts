import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { TransactionListState, TransactionSummaryState } from '@expensely/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './pages';
import { RecentTransactionListComponent, TransactionSummaryComponent } from './components';

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent, RecentTransactionListComponent],
  imports: [SharedModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionListState, TransactionSummaryState])]
})
export class DashboardModule {}
