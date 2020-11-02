import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MainLayoutComponent, EmptyLayoutComponent, NavBarComponent } from './components';

@NgModule({
  declarations: [EmptyLayoutComponent, MainLayoutComponent, NavBarComponent],
  imports: [RouterModule, HttpClientModule],
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
