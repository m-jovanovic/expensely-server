import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';

const routes: Routes = [
  {
    path: 'create',
    component: CreateTransactionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionsRoutingModule {}
