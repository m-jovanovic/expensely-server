import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { catchError, tap, filter } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCodes, PasswordValidators, RouterService } from '@expensely/core';

@Component({
  selector: 'exp-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  requestSent = false;
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
    if (this.requestSent) {
      return;
    }

    this.submitted = true;
    this.emailAlreadyInUse = false;

    if (this.registerForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.setRequestSentToTrueAndDisableRegisterForm();

    this.authenticationFacade
      .register(
        this.registerForm.value.firstName,
        this.registerForm.value.lastName,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.registerForm.value.confirmationPassword
      )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleRegisterError(new ApiErrorResponse(error));

          return of(true);
        }),
        tap(() => {
          this.submitted = false;

          this.setRequestSentToFalseAndEnableRegisterForm();
        }),
        filter(() => !this.emailAlreadyInUse),
        tap(() => {
          this.setRequestSentToTrueAndDisableRegisterForm();

          return this.authenticationFacade.login(this.registerForm.value.email, this.registerForm.value.password).pipe(
            catchError(() => {
              this.redirectToLogin();

              return of(true);
            }),
            tap(() => {
              this.setRequestSentToFalseAndEnableRegisterForm();
            })
          );
        })
      )
      .subscribe();
  }

  setRequestSentToTrueAndDisableRegisterForm(): void {
    this.requestSent = true;
    this.registerForm.disable();
  }

  setRequestSentToFalseAndEnableRegisterForm(): void {
    this.requestSent = false;
    this.registerForm.enable();
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
