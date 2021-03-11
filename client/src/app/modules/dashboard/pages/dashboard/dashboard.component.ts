import { Component, OnInit } from '@angular/core';

import { RouterService, TransactionListFacade, TransactionSummaryFacade } from '@expensely/core';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private readonly numberOfTransactions = 10;

  constructor(
    public transactionListFacade: TransactionListFacade,
    public transactionSummaryFacade: TransactionSummaryFacade,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.transactionListFacade.loadTransactions(this.numberOfTransactions);

    this.transactionSummaryFacade.loadTransactionSummary();
  }

  async selectTransaction(transactionId: string): Promise<boolean> {
    return await this.routerService.navigate(['/transactions', transactionId]);
  }
}
