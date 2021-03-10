import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { TransactionState, TransactionListState, CategoryState, CurrencyState } from '@expensely/core/store';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent, TransactionDetailsComponent, TransactionListComponent } from './pages';
import { DeleteTransactionButtonComponent, TransactionInformationComponent, TransactionListItemComponent } from './components';

@NgModule({
  declarations: [
    CreateTransactionComponent,
    TransactionListComponent,
    TransactionListItemComponent,
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
