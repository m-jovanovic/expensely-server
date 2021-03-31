import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { BudgetState, CategoryState, UserState } from '@expensely/core';
import { BudgetsRoutingModule } from './budgets-routing.module';

@NgModule({
  declarations: [],
  imports: [SharedModule, ReactiveFormsModule, BudgetsRoutingModule, NgxsModule.forFeature([BudgetState, CategoryState, UserState])]
})
export class BudgetsModule {}
