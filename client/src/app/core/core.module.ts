import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EmptyLayoutComponent } from './components';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';

@NgModule({
  declarations: [EmptyLayoutComponent, MainLayoutComponent],
  imports: [RouterModule, HttpClientModule],
  providers: [],
  exports: [EmptyLayoutComponent]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('The Core module has already been loaded.');
    }
  }
}
