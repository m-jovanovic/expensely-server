import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, ErrorCode, RouterService } from '@expensely/core';

@Component({
  selector: 'exp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private requestSent = false;
  loginForm: FormGroup;
  isLoading$: Observable<boolean>;
  submitted = false;
  invalidEmailOrPassword = false;

  constructor(private formBuilder: FormBuilder, private authenticationFacade: AuthenticationFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    this.isLoading$ = this.authenticationFacade.isLoading$;
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
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.loginForm.enable();
        })
      )
      .subscribe(
        () => {},
        (error: ApiErrorResponse) => this.handleLoginError(error)
      );
  }

  async redirectToRegister(): Promise<boolean> {
    return await this.routerService.navigateByUrl('/register');
  }

  handleLoginError(errorResponse: ApiErrorResponse): void {
    if (errorResponse.hasError(ErrorCode.UserEmailOrPasswordInvalid)) {
      this.invalidEmailOrPassword = true;
    }
  }
}
