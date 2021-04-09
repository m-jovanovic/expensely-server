import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  ConfirmationDialogComponent,
  LanguagePickerComponent,
  NotificationDialogComponent,
  SpinnerComponent,
  ReturnComponent
} from './components';
import { TranslocoRootModule } from '@expensely/transloco/transloco-root.module';
import { TrapFocusDirective } from './directives';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    ConfirmationDialogComponent,
    SpinnerComponent,
    TrapFocusDirective,
    NotificationDialogComponent,
    LanguagePickerComponent,
    ReturnComponent
  ],
  imports: [CommonModule, TranslocoRootModule, RouterModule],
  exports: [CommonModule, TranslocoRootModule, TrapFocusDirective, SpinnerComponent, LanguagePickerComponent, ReturnComponent]
})
export class SharedModule {}
