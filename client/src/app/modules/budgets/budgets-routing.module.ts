import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreateBudgetComponent } from './pages';

const routes: Routes = [
  {
    path: 'create',
    component: CreateBudgetComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BudgetsRoutingModule {}
