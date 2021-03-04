import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { SharedModule } from '@expensely/shared';
import { AuthenticationState, CurrencyState, UserState } from '@expensely/core';
import { SetupRoutingModule } from './setup-routing.module';
import { SetupComponent } from './pages/setup/setup.component';
import { SetupPrimaryCurrencyComponent } from './components/setup-primary-currency/setup-primary-currency.component';

@NgModule({
  declarations: [SetupComponent, SetupPrimaryCurrencyComponent],
  imports: [SharedModule, ReactiveFormsModule, SetupRoutingModule, NgxsModule.forFeature([AuthenticationState, CurrencyState, UserState])]
})
export class SetupModule {}
