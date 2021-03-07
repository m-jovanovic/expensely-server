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
    public transactionListFacade: TransactionListFacade,
    public transactionSummaryFacade: TransactionSummaryFacade,
    private confirmationDialogService: ConfirmationDialogService
  ) {}

  ngOnInit(): void {
    this.transactionListFacade.loadTransactions(this.numberOfTransactions);

    this.transactionSummaryFacade.loadTransactionSummary();
  }

  deleteTransaction(transactionId: string): void {
    this.confirmationDialogService.open({
      message: 'Are you sure you want to delete this transaction?',
      cancelButtonText: 'Cancel',
      confirmButtonText: 'Delete'
    });

    this.confirmationDialogService.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.transactionListFacade.deleteTransaction(transactionId);
    });
  }
}
