import { NgModule } from '@angular/core';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { LoginComponent, RegisterComponent } from './components';

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [AuthenticationRoutingModule],
})
export class AuthenticationModule { }
