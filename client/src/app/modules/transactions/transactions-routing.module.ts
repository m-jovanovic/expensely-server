import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreateTransactionComponent, TransactionDetailsComponent, TransactionListComponent, UpdateTransactionComponent } from './pages';

const routes: Routes = [
  {
    path: '',
    component: TransactionListComponent
  },
  {
    path: 'create',
    component: CreateTransactionComponent
  },
  {
    path: ':id',
    component: TransactionDetailsComponent
  },
  {
    path: ':id/update',
    component: UpdateTransactionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionsRoutingModule {}
