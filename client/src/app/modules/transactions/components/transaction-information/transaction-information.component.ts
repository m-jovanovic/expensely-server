import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transaction-information',
  templateUrl: './transaction-information.component.html',
  styleUrls: ['./transaction-information.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionInformationComponent {
  @Input()
  transaction: TransactionResponse;

  constructor() {}
}
