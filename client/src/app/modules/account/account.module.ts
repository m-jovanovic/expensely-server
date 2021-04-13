import { NgModule } from '@angular/core';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';

export const accountTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [],
  imports: [SharedModule],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'account',
        loader: accountTranslationsLoader
      }
    }
  ]
})
export class AccountModule {}
