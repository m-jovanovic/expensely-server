import { Component, OnInit } from '@angular/core';

import { TransactionFacade, TransactionSummaryFacade } from '@expensely/core';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private readonly numberOfTransactions = 10;

  constructor(public transactionFacade: TransactionFacade, public transactionSummaryFacade: TransactionSummaryFacade) {}

  ngOnInit(): void {
    this.transactionFacade.loadTransactions(this.numberOfTransactions);

    this.transactionSummaryFacade.loadTransactionSummary();
  }

  deleteTransaction(transactionId: string): void {
    this.transactionFacade.deleteTransaction(transactionId);
  }
}
