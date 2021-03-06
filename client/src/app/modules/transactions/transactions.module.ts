import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { TransactionState, TransactionListState, CategoryState, CurrencyState } from '@expensely/core/store';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';
import { TransactionComponent } from './pages/transaction/transaction.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from './pages/transaction-details/transaction-details.component';

@NgModule({
  declarations: [CreateTransactionComponent, TransactionComponent, TransactionListComponent, TransactionDetailsComponent],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TransactionsRoutingModule,
    NgxsModule.forFeature([TransactionState, TransactionListState, CategoryState, CurrencyState])
  ]
})
export class TransactionsModule {}
