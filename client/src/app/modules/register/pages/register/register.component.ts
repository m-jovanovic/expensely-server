import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { finalize, concatMap } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCode, PasswordValidators, RouterService } from '@expensely/core';

@Component({
  selector: 'exp-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading$: Observable<boolean>;
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

    this.isLoading$ = this.authenticationFacade.isLoading$;
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
        concatMap(() => this.authenticationFacade.login(this.registerForm.value.email, this.registerForm.value.password)),
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.registerForm.enable();
        })
      )
      .subscribe(
        () => {},
        (error: ApiErrorResponse) => this.handleRegisterError(error)
      );
  }

  setRequestSentToTrueAndDisableRegisterForm(): void {
    this.requestSent = true;
    this.registerForm.disable();
  }

  handleRegisterError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasError(ErrorCode.UserEmailAlreadyInUse)) {
      this.emailAlreadyInUse = true;
    } else {
      this.redirectToLogin();
    }
  }

  async redirectToLogin(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/login');
  }
}
