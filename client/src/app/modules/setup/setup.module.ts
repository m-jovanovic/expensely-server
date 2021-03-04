import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { AuthenticationState, CurrencyState } from '@expensely/core';
import { SetupRoutingModule } from './setup-routing.module';
import { SetupComponent } from './pages/setup/setup.component';

@NgModule({
  declarations: [SetupComponent],
  imports: [SharedModule, ReactiveFormsModule, SetupRoutingModule, NgxsModule.forFeature([AuthenticationState, CurrencyState])]
})
export class SetupModule {}
