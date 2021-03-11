import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

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

  constructor() {}

  ngOnInit(): void {}
}
