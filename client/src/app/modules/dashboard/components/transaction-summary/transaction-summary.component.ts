import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'exp-transaction-summary',
  templateUrl: './transaction-summary.component.html',
  styleUrls: ['./transaction-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionSummaryComponent {
  @Input()
  expense: string;

  @Input()
  income: string;

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  constructor() {}
}
