import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { concatMap, finalize } from 'rxjs/operators';

import { ApiErrorResponse, AuthenticationFacade, CurrencyFacade, CurrencyResponse, RouterService, UserFacade } from '@expensely/core';

@Component({
  selector: 'exp-setup-primary-currency',
  templateUrl: './setup-primary-currency.component.html',
  styleUrls: ['./setup-primary-currency.component.scss']
})
export class SetupPrimaryCurrencyComponent implements OnInit {
  private requestSent = false;
  currencies$: Observable<CurrencyResponse[]>;
  isLoading$: Observable<boolean>;
  setupForm: FormGroup;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private authenticationFacade: AuthenticationFacade,
    private userFacade: UserFacade,
    private currencyFacade: CurrencyFacade,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.setupForm = this.formBuilder.group({
      currency: ['', Validators.required]
    });

    if (this.authenticationFacade.userPrimaryCurrency > 0) {
      this.redirectToDashboard();

      return;
    }

    this.currencies$ = this.currencyFacade.currencies$;

    this.currencyFacade.loadCurrencies();

    this.isLoading$ = this.userFacade.isLoading$;
  }

  onSubmit(): void {
    if (this.requestSent) {
      return;
    }

    this.submitted = true;

    if (this.setupForm.invalid) {
      this.requestSent = false;

      return;
    }

    this.requestSent = true;
    this.setupForm.disable();

    this.userFacade
      .addUserCurrency(this.setupForm.value.currency)
      .pipe(
        concatMap(() => this.authenticationFacade.refreshToken()),
        finalize(() => {
          this.submitted = false;
          this.requestSent = false;
          this.setupForm.enable();
        })
      )
      .subscribe(
        () => this.redirectToDashboard(),
        (error: ApiErrorResponse) => this.handleError(error)
      );
  }

  handleError(errorResponse: ApiErrorResponse): void {
    // TODO: Handle possible errors.
    console.log('Failed to add currency.');
  }

  private redirectToDashboard(): void {
    this.routerService.navigateByUrl('/dashboard');
  }
}
