import { ModuleWithProviders, NgModule } from '@angular/core';
import { NGXS_PLUGINS } from '@ngxs/store';

import { LogoutPlugin } from './logout-plugin';

@NgModule()
export class NgxsLogoutPluginModule {
  static forRoot(): ModuleWithProviders<NgxsLogoutPluginModule> {
    return {
      ngModule: NgxsLogoutPluginModule,
      providers: [
        {
          provide: NGXS_PLUGINS,
          useClass: LogoutPlugin,
          multi: true
        }
      ]
    };
  }
}
