import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '@expensely/shared';
import { RegisterComponent } from './pages';
import { RegisterRoutingModule } from './register-routing.module';

@NgModule({
  declarations: [RegisterComponent],
  imports: [SharedModule, ReactiveFormsModule, RegisterRoutingModule]
})
export class RegisterModule {}
