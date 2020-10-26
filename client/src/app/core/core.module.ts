import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EmptyLayoutComponent } from './components';

@NgModule({
  declarations: [EmptyLayoutComponent],
  imports: [RouterModule],
  providers: [],
  exports: [EmptyLayoutComponent],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('The Core module has already been loaded.');
    }
  }
}
