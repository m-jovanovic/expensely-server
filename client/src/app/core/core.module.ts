import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import {
  AccountDropdownComponent,
  MainLayoutComponent,
  EmptyLayoutComponent,
  NavBarComponent,
  SideNavigationComponent
} from './components';

@NgModule({
  declarations: [AccountDropdownComponent, EmptyLayoutComponent, MainLayoutComponent, NavBarComponent, SideNavigationComponent],
  imports: [CommonModule, RouterModule, HttpClientModule],
  providers: [],
  exports: [MainLayoutComponent, EmptyLayoutComponent]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('The Core module has already been loaded.');
    }
  }
}
