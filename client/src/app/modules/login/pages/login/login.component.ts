import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCodes, RouterService } from '@expensely/core';

@Component({
  selector: 'exp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  requestSent = false;
  invalidEmailOrPassword = false;

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;
    this.invalidEmailOrPassword = false;

    if (this.loginForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.loginForm.disable();

    this.authenticationFacade
      .login(this.loginForm.value.email, this.loginForm.value.password)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleLoginError(new ApiErrorResponse(error));

          return of(true);
        }),
        tap(() => {
          this.submitted = false;
          this.requestSent = false;
          this.loginForm.enable();
        })
      )
      .subscribe();
  }

  async redirectToRegister(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/register');
  }

  handleLoginError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasError(ErrorCodes.UserEmailOrPasswordInvalid)) {
      this.invalidEmailOrPassword = true;
    }
  }
}
