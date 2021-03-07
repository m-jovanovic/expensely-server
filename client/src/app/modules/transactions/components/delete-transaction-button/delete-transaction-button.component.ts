import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

import { ConfirmationDialogService } from '@expensely/shared/services';

@Component({
  selector: 'exp-delete-transaction-button',
  templateUrl: './delete-transaction-button.component.html',
  styleUrls: ['./delete-transaction-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeleteTransactionButtonComponent {
  @Input()
  transactionId: string;

  @Input()
  displayText: boolean = true;

  @Input()
  isLoading: boolean;

  @Output()
  transactionDeletedEvent = new EventEmitter<string>();

  constructor(private confirmationDialogService: ConfirmationDialogService) {}

  deleteTransaction(): void {
    if (this.isLoading) {
      return;
    }

    this.confirmationDialogService.open({
      message: 'Are you sure you want to delete this transaction?',
      cancelButtonText: 'Cancel',
      confirmButtonText: 'Delete'
    });

    this.confirmationDialogService.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.transactionDeletedEvent.emit(this.transactionId);
    });
  }
}
