import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { NgxsModule } from '@ngxs/store';
import { TransactionSummaryState } from '@expensely/core';

@NgModule({
  declarations: [DashboardComponent],
  imports: [CommonModule, DashboardRoutingModule, NgxsModule.forFeature([TransactionSummaryState])]
})
export class DashboardModule {}
