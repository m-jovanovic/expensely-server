import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ConfirmationDialogData } from './confirmation-dialog.data';

@Component({
  selector: 'exp-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss']
})
export class ConfirmationDialogComponent implements OnInit {
  @Input()
  data: ConfirmationDialogData;

  @Output()
  confirmationEvent = new EventEmitter<boolean>();

  constructor() {}

  ngOnInit(): void {}

  confirm(mouseEvent: MouseEvent): void {
    mouseEvent.stopPropagation();

    this.confirmationEvent.emit(true);
  }

  cancel(mouseEvent: MouseEvent): void {
    mouseEvent.stopPropagation();

    this.confirmationEvent.emit(false);
  }

  dismiss(): void {
    this.confirmationEvent.emit(false);
  }

  stopPropagation(mouseEvent: MouseEvent): void {
    mouseEvent.stopPropagation();
  }
}
