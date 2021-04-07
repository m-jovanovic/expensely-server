import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BudgetDetailsComponent, CreateBudgetComponent, UpdateBudgetComponent } from './pages';

const routes: Routes = [
  {
    path: 'create',
    component: CreateBudgetComponent
  },
  {
    path: ':id',
    component: BudgetDetailsComponent
  },
  {
    path: ':id/update',
    component: UpdateBudgetComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BudgetsRoutingModule {}
