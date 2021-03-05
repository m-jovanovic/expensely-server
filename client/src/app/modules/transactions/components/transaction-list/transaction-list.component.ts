import { Component, Input } from '@angular/core';

import { TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss']
})
export class TransactionListComponent {
  @Input()
  transactions: TransactionResponse[];

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  constructor() {}
}