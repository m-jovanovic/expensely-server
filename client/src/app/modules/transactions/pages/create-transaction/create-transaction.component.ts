import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { CategoryFacade } from '@expensely/core/store/category';
import { CurrencyFacade } from '@expensely/core/store/currency';
import { CategoryResponse } from '@expensely/core/contracts/transactions/category-response';
import { CurrencyResponse } from '@expensely/core/contracts/transactions/currency-response';

@Component({
  selector: 'exp-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss']
})
export class CreateTransactionComponent implements OnInit {
  categoriesIsLoading$: Observable<boolean>;
  categories$: Observable<CategoryResponse[]>;
  currenciesIsLoading$: Observable<boolean>;
  currencies$: Observable<CurrencyResponse[]>;

  constructor(private categoryFacade: CategoryFacade, private currencyFacade: CurrencyFacade) {}

  ngOnInit(): void {
    this.categoriesIsLoading$ = this.categoryFacade.isLoading$;

    this.categories$ = this.categoryFacade.categories$;

    this.currenciesIsLoading$ = this.currencyFacade.isLoading$;

    this.currencies$ = this.currencyFacade.currencies$;

    this.categoryFacade.loadCategories().subscribe();

    this.currencyFacade.loadCurrencies().subscribe();
  }
}
