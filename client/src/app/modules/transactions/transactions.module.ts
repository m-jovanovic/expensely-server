import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { CategoryState } from '@expensely/core/store/category';
import { CurrencyState } from '@expensely/core/store/currency';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent } from './pages/create-transaction/create-transaction.component';
import { CreateTransactionDetailsComponent } from './components/create-transaction-details/create-transaction-details.component';

@NgModule({
  declarations: [CreateTransactionComponent, CreateTransactionDetailsComponent],
  imports: [SharedModule, ReactiveFormsModule, TransactionsRoutingModule, NgxsModule.forFeature([CategoryState, CurrencyState])]
})
export class TransactionsModule {}
