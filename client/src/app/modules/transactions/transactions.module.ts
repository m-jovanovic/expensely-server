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
import { TransactionInformationComponent } from './components/transaction-information/transaction-information.component';
import { DeleteTransactionButtonComponent } from './components/delete-transaction-button/delete-transaction-button.component';

@NgModule({
  declarations: [
    CreateTransactionComponent,
    TransactionComponent,
    TransactionListComponent,
    TransactionDetailsComponent,
    TransactionInformationComponent,
    DeleteTransactionButtonComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TransactionsRoutingModule,
    NgxsModule.forFeature([TransactionState, TransactionListState, CategoryState, CurrencyState])
  ]
})
export class TransactionsModule {}
