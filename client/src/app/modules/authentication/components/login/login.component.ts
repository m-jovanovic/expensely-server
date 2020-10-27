import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCodes } from '@expensely/core';

@Component({
  selector: 'exp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = this.formBuilder.group({
    email: ['', Validators.email],
    password: ['', Validators.required]
  });

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade) {}

  ngOnInit(): void {}

  onSubmit(): void {
    // TODO: Add validation.
    const value = this.loginForm.value;

    this.authenticationFacade
      .login(value.email, value.password)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleLoginError(error.error);

          return of(true);
        })
      )
      .subscribe();
  }

  handleLoginError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle more errors.
    if (errorResponse.errors[0].code === ErrorCodes.UserEmailOrPasswordInvalid) {
      console.log('Invalid email or password.');
    }
  }
}
