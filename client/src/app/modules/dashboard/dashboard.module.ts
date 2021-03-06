import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { TransactionListState, TransactionSummaryState } from '@expensely/core';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TransactionSummaryComponent } from './components/transaction-summary/transaction-summary.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent, TransactionListComponent],
  imports: [SharedModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionListState, TransactionSummaryState])]
})
export class DashboardModule {}
