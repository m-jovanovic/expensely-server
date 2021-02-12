import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { TransactionResponse } from '@expensely/core/contracts/transactions/transaction-response';

@Component({
  selector: 'exp-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionListComponent implements OnInit {
  @Input()
  transactions: TransactionResponse[];

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  @Output()
  deleteTransactionEvent = new EventEmitter<string>();

  constructor() {}

  ngOnInit(): void {}

  deleteTransaction(transactionId: string): void {
    this.deleteTransactionEvent.emit(transactionId);
  }
}
