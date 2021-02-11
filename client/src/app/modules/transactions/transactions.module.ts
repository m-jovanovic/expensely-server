import { NgModule } from '@angular/core';
import { SharedModule } from '@expensely/shared';

import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';

@NgModule({
  declarations: [CreateTransactionComponent],
  imports: [SharedModule, TransactionsRoutingModule]
})
export class TransactionsModule {}
