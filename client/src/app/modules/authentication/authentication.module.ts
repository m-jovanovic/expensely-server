import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthenticationLayoutComponent, LoginComponent, RegisterComponent } from './components';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, AuthenticationLayoutComponent],
  imports: [CommonModule, ReactiveFormsModule, AuthenticationRoutingModule]
})
export class AuthenticationModule {}
