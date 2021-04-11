import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  ConfirmationDialogComponent,
  LanguagePickerComponent,
  NotificationDialogComponent,
  SpinnerComponent,
  ReturnComponent
} from './components';
import { TrapFocusDirective } from './directives';
import { RouterModule } from '@angular/router';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
  declarations: [
    ConfirmationDialogComponent,
    SpinnerComponent,
    TrapFocusDirective,
    NotificationDialogComponent,
    LanguagePickerComponent,
    ReturnComponent
  ],
  imports: [CommonModule, TranslocoModule, RouterModule],
  exports: [CommonModule, TranslocoModule, TrapFocusDirective, SpinnerComponent, LanguagePickerComponent, ReturnComponent]
})
export class SharedModule {}
