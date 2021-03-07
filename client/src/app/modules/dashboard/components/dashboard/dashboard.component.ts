import { Component, OnInit } from '@angular/core';

import { TransactionListFacade, TransactionSummaryFacade } from '@expensely/core';
import { ConfirmationDialogService } from '@expensely/shared/services';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private readonly numberOfTransactions = 10;

  constructor(
    public transactionFacade: TransactionListFacade,
    public transactionSummaryFacade: TransactionSummaryFacade,
    private confirmationDialogService: ConfirmationDialogService
  ) {}

  ngOnInit(): void {
    this.transactionFacade.loadTransactions(this.numberOfTransactions);

    this.transactionSummaryFacade.loadTransactionSummary();
  }

  deleteTransaction(transactionId: string): void {
    this.confirmationDialogService.open({
      message: 'Are you sure you want to remove this transaction?',
      cancelButtonText: 'Cancel',
      confirmButtonText: 'Remove'
    });

    this.confirmationDialogService.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.transactionFacade.deleteTransaction(transactionId);
    });
  }
}
