import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';

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

  constructor(private confirmationDialogService: ConfirmationDialogService, private translationService: TranslocoService) {}

  deleteTransaction(): void {
    if (this.isLoading) {
      return;
    }

    this.confirmationDialogService.open({
      message: this.translationService.translate('transactions.delete.dialog.message'),
      cancelButtonText: this.translationService.translate('transactions.delete.dialog.cancelButtonText'),
      confirmButtonText: this.translationService.translate('transactions.delete.dialog.confirmButtonText')
    });

    this.confirmationDialogService.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.transactionDeletedEvent.emit(this.transactionId);
    });
  }
}
