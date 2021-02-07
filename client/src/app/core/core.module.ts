import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '@expensely/shared';
import {
  AccountDropdownComponent,
  MainLayoutComponent,
  EmptyLayoutComponent,
  NavBarComponent,
  SideNavigationComponent
} from './components';
import { interceptorsProvider } from './interceptors';

@NgModule({
  declarations: [AccountDropdownComponent, EmptyLayoutComponent, MainLayoutComponent, NavBarComponent, SideNavigationComponent],
  imports: [SharedModule, RouterModule, HttpClientModule],
  providers: [interceptorsProvider],
  exports: [MainLayoutComponent, EmptyLayoutComponent]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('The Core module has already been loaded.');
    }
  }
}
