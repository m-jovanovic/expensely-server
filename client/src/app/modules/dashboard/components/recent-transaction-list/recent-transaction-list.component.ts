import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { TransactionResponse } from '@expensely/core/contracts/transactions/transaction-response';

@Component({
  selector: 'exp-recent-transaction-list',
  templateUrl: './recent-transaction-list.component.html',
  styleUrls: ['./recent-transaction-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecentTransactionListComponent implements OnInit {
  @Input()
  transactions: TransactionResponse[];

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  @Output()
  transactionSelectedEvent = new EventEmitter<string>();

  constructor() {}

  ngOnInit(): void {}

  selectTransaction(transactionId: string): void {
    this.transactionSelectedEvent.emit(transactionId);
  }
}
