import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationDialogComponent, SpinnerComponent } from './components';

@NgModule({
  declarations: [ConfirmationDialogComponent, SpinnerComponent],
  imports: [CommonModule],
  exports: [CommonModule, SpinnerComponent]
})
export class SharedModule {}
