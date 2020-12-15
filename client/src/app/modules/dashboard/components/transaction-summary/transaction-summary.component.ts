import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'exp-transaction-summary',
  templateUrl: './transaction-summary.component.html',
  styleUrls: ['./transaction-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionSummaryComponent implements OnInit {
  @Input()
  title: string;

  @Input()
  formattedTransaction: string;

  @Input()
  timePeriod: string;

  @Input()
  isLoading: boolean;

  @Input()
  error: boolean;

  constructor() {}

  ngOnInit(): void {}
}
