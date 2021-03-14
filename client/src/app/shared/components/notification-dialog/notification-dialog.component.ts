import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'exp-notification-dialog',
  templateUrl: './notification-dialog.component.html',
  styleUrls: ['./notification-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationDialogComponent {
  @Input()
  message: string;

  @Output()
  closed = new EventEmitter<any>(null);

  constructor() {}

  close(): void {
    this.closed.emit();
  }
}
