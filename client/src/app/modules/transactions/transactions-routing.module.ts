import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TransactionComponent } from './pages/transaction/transaction.component';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';
import { TransactionDetailsComponent } from './pages/transaction-details/transaction-details.component';

const routes: Routes = [
  {
    path: '',
    component: TransactionComponent
  },
  {
    path: 'create',
    component: CreateTransactionComponent
  },
  {
    path: ':id',
    component: TransactionDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionsRoutingModule {}
