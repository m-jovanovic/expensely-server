import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationDialogComponent, SpinnerComponent } from './components';
import { TranslocoRootModule } from '@expensely/transloco/transloco-root.module';

@NgModule({
  declarations: [ConfirmationDialogComponent, SpinnerComponent],
  imports: [CommonModule, TranslocoRootModule],
  exports: [CommonModule, TranslocoRootModule, SpinnerComponent]
})
export class SharedModule {}
