import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationDialogComponent, LanguagePickerComponent, NotificationDialogComponent, SpinnerComponent } from './components';
import { TranslocoRootModule } from '@expensely/transloco/transloco-root.module';
import { TrapFocusDirective } from './directives';

@NgModule({
  declarations: [ConfirmationDialogComponent, SpinnerComponent, TrapFocusDirective, NotificationDialogComponent, LanguagePickerComponent],
  imports: [CommonModule, TranslocoRootModule],
  exports: [CommonModule, TranslocoRootModule, TrapFocusDirective, SpinnerComponent, LanguagePickerComponent]
})
export class SharedModule {}
