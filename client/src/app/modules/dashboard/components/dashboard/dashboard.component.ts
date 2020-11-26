import { Component, OnInit } from '@angular/core';

import { TransactionSummaryFacade } from '@expensely/core';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  constructor(private transactionSummaryFacade: TransactionSummaryFacade) {}

  ngOnInit(): void {
    this.transactionSummaryFacade.loadTransactionSummary().subscribe();
  }

  get f(): TransactionSummaryFacade {
    return this.transactionSummaryFacade;
  }
}
