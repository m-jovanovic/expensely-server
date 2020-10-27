import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { LoginComponent, RegisterComponent } from './components';

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [ReactiveFormsModule, AuthenticationRoutingModule]
})
export class AuthenticationModule {}
