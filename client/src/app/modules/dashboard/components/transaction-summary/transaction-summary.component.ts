import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'exp-transaction-summary',
  templateUrl: './transaction-summary.component.html',
  styleUrls: ['./transaction-summary.component.scss']
})
export class TransactionSummaryComponent implements OnInit {
  @Input()
  isLoading: boolean;

  @Input()
  title: string;

  @Input()
  formattedTransaction: string;

  @Input()
  timePeriod: string;

  constructor() {}

  ngOnInit(): void {}
}
