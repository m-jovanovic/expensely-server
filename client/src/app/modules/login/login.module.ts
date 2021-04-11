import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { LoginComponent } from './pages';
import { LoginRoutingModule } from './login-routing.module';

export const loginTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [LoginComponent],
  imports: [SharedModule, ReactiveFormsModule, LoginRoutingModule],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'login',
        loader: loginTranslationsLoader
      }
    }
  ]
})
export class LoginModule {}
