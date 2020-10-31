import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCodes, RouterService } from '@expensely/core';

@Component({
  selector: 'exp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  invalidEmailOrPassword = false;

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    this.submitted = true;
    this.invalidEmailOrPassword = false;

    if (this.loginForm.invalid) {
      return;
    }

    const value = this.loginForm.value;

    this.authenticationFacade
      .login(value.email, value.password)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleLoginError(new ApiErrorResponse(error));

          return of(true);
        })
      )
      .subscribe(() => (this.submitted = false));
  }

  redirectToRegister(): void {
    this.routerService.navigate(['/register']);
  }

  handleLoginError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasError(ErrorCodes.UserEmailOrPasswordInvalid)) {
      this.invalidEmailOrPassword = true;
    }
  }
}
