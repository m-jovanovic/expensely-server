import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { TransactionState, TransactionListState, CategoryState, UserState } from '@expensely/core/store';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { CreateTransactionComponent, TransactionDetailsComponent, TransactionListComponent, UpdateTransactionComponent } from './pages';
import { DeleteTransactionButtonComponent, TransactionInformationComponent, TransactionListItemComponent } from './components';

export const transactionsTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [
    CreateTransactionComponent,
    TransactionListComponent,
    TransactionListItemComponent,
    TransactionDetailsComponent,
    TransactionInformationComponent,
    DeleteTransactionButtonComponent,
    UpdateTransactionComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TransactionsRoutingModule,
    NgxsModule.forFeature([TransactionState, TransactionListState, CategoryState, UserState])
  ],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'transactions',
        loader: transactionsTranslationsLoader
      }
    }
  ]
})
export class TransactionsModule {}
