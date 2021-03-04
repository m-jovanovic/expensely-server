import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { CurrencyFacade, CurrencyResponse } from '@expensely/core';

@Component({
  selector: 'exp-setup',
  templateUrl: './setup.component.html',
  styleUrls: ['./setup.component.scss']
})
export class SetupComponent implements OnInit {
  currencies$: Observable<CurrencyResponse[]>;
  isLoading$: Observable<boolean>;

  constructor(private currencyFacade: CurrencyFacade) {}

  ngOnInit(): void {
    this.currencyFacade.loadCurrencies();

    this.currencies$ = this.currencyFacade.currencies$;

    this.isLoading$ = this.currencyFacade.isLoading$;
  }
}
