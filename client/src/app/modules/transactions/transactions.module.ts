import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { TransactionState, CategoryState, CurrencyState } from '@expensely/core/store';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';
import { TransactionComponent } from './pages/transaction/transaction.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';

@NgModule({
  declarations: [CreateTransactionComponent, TransactionComponent, TransactionListComponent],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TransactionsRoutingModule,
    NgxsModule.forFeature([TransactionState, CategoryState, CurrencyState])
  ]
})
export class TransactionsModule {}
