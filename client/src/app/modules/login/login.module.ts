import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '@expensely/shared';
import { LoginComponent } from './pages';
import { LoginRoutingModule } from './login-routing.module';

@NgModule({
  declarations: [LoginComponent],
  imports: [SharedModule, ReactiveFormsModule, LoginRoutingModule]
})
export class LoginModule {}
