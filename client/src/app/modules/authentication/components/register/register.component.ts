import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCodes, RouterService } from '@expensely/core';
import { PasswordValidators } from '../../validation/password-validators';

@Component({
  selector: 'exp-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  emailAlreadyInUse = false;

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), PasswordValidators.passwordStrength]],
      confirmationPassword: ['', [Validators.required, PasswordValidators.confirmationPasswordMustMatch]]
    });
  }

  onSubmit(): void {
    this.submitted = true;
    this.emailAlreadyInUse = false;

    if (this.registerForm.invalid) {
      return;
    }

    const value = this.registerForm.value;

    this.authenticationFacade
      .register(value.firstName, value.lastName, value.email, value.password, value.confirmationPassword)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleRegisterError(new ApiErrorResponse(error));

          return of(true);
        })
      )
      .subscribe(() => {
        this.submitted = false;

        this.authenticationFacade
          .login(value.email, value.password)
          .pipe(
            catchError(() => {
              this.redirectToLogin();

              return of(true);
            })
          )
          .subscribe();
      });
  }

  handleRegisterError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasError(ErrorCodes.UserEmailAlreadyInUse)) {
      this.emailAlreadyInUse = true;
    }
  }

  redirectToLogin(): void {
    this.routerService.navigate(['/login']);
  }
}
