import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TransactionComponent } from './pages/transaction/transaction.component';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';

const routes: Routes = [
  {
    path: '',
    component: TransactionComponent
  },
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
