import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxsModule } from '@ngxs/store';

import { TransactionState, TransactionSummaryState } from '@expensely/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TransactionSummaryComponent } from './components/transaction-summary/transaction-summary.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent, TransactionListComponent],
  imports: [CommonModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionState, TransactionSummaryState])]
})
export class DashboardModule {}
