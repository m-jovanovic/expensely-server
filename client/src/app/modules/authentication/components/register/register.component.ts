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
    if (this.submitted) {
      return;
    }

    this.submitted = true;
    this.emailAlreadyInUse = false;

    if (this.registerForm.invalid) {
      this.submitted = false;

      return;
    }

    const form = this.registerForm.value;

    this.authenticationFacade
      .register(form.firstName, form.lastName, form.email, form.password, form.confirmationPassword)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleRegisterError(new ApiErrorResponse(error));

          this.submitted = false;

          return of(true);
        }),
        filter(() => !this.emailAlreadyInUse),
        tap(() => {
          this.submitted = false;

          return this.authenticationFacade.login(form.email, form.password).pipe(
            catchError(() => {
              this.redirectToLogin();

              return of(true);
            })
          );
        })
      )
      .subscribe();
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
