import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxsModule } from '@ngxs/store';

import { TransactionSummaryState } from '@expensely/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TransactionSummaryComponent } from './components/transaction-summary/transaction-summary.component';

@NgModule({
  declarations: [DashboardComponent, TransactionSummaryComponent],
  imports: [CommonModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionSummaryState])]
})
export class DashboardModule {}
