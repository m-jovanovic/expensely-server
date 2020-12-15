import { Component, OnInit } from '@angular/core';

import { TransactionFacade, TransactionSummaryFacade } from '@expensely/core';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private readonly numberOfTransactions = 10;

  constructor(private transactionFacade: TransactionFacade, private transactionSummaryFacade: TransactionSummaryFacade) {}

  ngOnInit(): void {
    this.transactionFacade.loadTransactions(this.numberOfTransactions).subscribe();
    this.transactionSummaryFacade.loadTransactionSummary().subscribe();
  }

  get transaction(): TransactionFacade {
    return this.transactionFacade;
  }

  get transactionSummary(): TransactionSummaryFacade {
    return this.transactionSummaryFacade;
  }
}
