import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '@expensely/shared';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthenticationLayoutComponent, LoginComponent, RegisterComponent } from './components';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, AuthenticationLayoutComponent],
  imports: [SharedModule, ReactiveFormsModule, AuthenticationRoutingModule]
})
export class AuthenticationModule {}
