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
  invalidEmailOrPassword = false;

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.submitted) {
      return;
    }

    this.submitted = true;
    this.invalidEmailOrPassword = false;

    if (this.loginForm.invalid) {
      this.submitted = false;

      return;
    }

    const form = this.loginForm.value;

    this.authenticationFacade
      .login(form.email, form.password)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleLoginError(new ApiErrorResponse(error));

          return of(true);
        }),
        tap(() => (this.submitted = false))
      )
      .subscribe();
  }

  redirectToRegister(): void {
    this.routerService.navigate(['/register']);
  }

  handleLoginError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasErrors()) {
      this.invalidEmailOrPassword = true;
    }
  }
}
