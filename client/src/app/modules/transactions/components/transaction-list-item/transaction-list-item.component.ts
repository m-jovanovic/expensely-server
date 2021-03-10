import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transaction-list-item',
  templateUrl: './transaction-list-item.component.html',
  styleUrls: ['./transaction-list-item.component.scss']
})
export class TransactionListItemComponent {
  @Input()
  transaction: TransactionResponse;

  @Output()
  transactionSelectedEvent = new EventEmitter<string>();

  constructor() {}

  selectTransaction(): void {
    this.transactionSelectedEvent.emit(this.transaction.id);
  }
}
