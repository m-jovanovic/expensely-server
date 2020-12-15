import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

import { TransactionResponse } from '@expensely/core/contracts/transaction/transaction-response';

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

  constructor() {}

  ngOnInit(): void {}
}
