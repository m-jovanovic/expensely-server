import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationDialogComponent, NotificationDialogComponent, SpinnerComponent } from './components';
import { TranslocoRootModule } from '@expensely/transloco/transloco-root.module';
import { TrapFocusDirective } from './directives';

@NgModule({
  declarations: [ConfirmationDialogComponent, SpinnerComponent, TrapFocusDirective, NotificationDialogComponent],
  imports: [CommonModule, TranslocoRootModule],
  exports: [CommonModule, TranslocoRootModule, SpinnerComponent, TrapFocusDirective]
})
export class SharedModule {}
