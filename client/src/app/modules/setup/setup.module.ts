import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { AuthenticationState, CurrencyState, UserState } from '@expensely/core';
import { SetupRoutingModule } from './setup-routing.module';
import { SetupComponent } from './pages/setup/setup.component';
import { SetupPrimaryCurrencyComponent } from './components/setup-primary-currency/setup-primary-currency.component';

export const setupTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [SetupComponent, SetupPrimaryCurrencyComponent],
  imports: [SharedModule, ReactiveFormsModule, SetupRoutingModule, NgxsModule.forFeature([AuthenticationState, CurrencyState, UserState])],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'setup',
        loader: setupTranslationsLoader
      }
    }
  ]
})
export class SetupModule {}
