import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { RegisterComponent } from './pages';
import { RegisterRoutingModule } from './register-routing.module';

export const registerTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [RegisterComponent],
  imports: [SharedModule, ReactiveFormsModule, RegisterRoutingModule],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'register',
        loader: registerTranslationsLoader
      }
    }
  ]
})
export class RegisterModule {}
