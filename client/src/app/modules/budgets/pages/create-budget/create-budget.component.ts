import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { BudgetFacade, CategoryFacade, CategoryResponse, CurrencyResponse, RouterService, UserFacade } from '@expensely/core';

@Component({
  selector: 'exp-create-budget',
  templateUrl: './create-budget.component.html',
  styleUrls: ['./create-budget.component.scss']
})
export class CreateBudgetComponent implements OnInit {
  createBudgetForm: FormGroup;
  currencies$: Observable<CurrencyResponse[]>;
  categories$: Observable<CategoryResponse[]>;
  isLoading$: Observable<boolean>;
  submitted = false;

  constructor(
    private budgetFacade: BudgetFacade,
    private userFacade: UserFacade,
    private categoryFacade: CategoryFacade,
    private formBuilder: FormBuilder,
    private routerService: RouterService
  ) {}

  ngOnInit(): void {
    this.createBudgetForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      amount: ['0.00', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      categories: ['', Validators.required],
      startDate: [this.getCurrentDateString(), Validators.required],
      endDate: [this.getCurrentDateString(), Validators.required]
    });

    this.currencies$ = this.userFacade.currencies$;

    this.categories$ = this.categoryFacade.expenseCategories$;

    this.isLoading$ = this.budgetFacade.isLoading$;

    this.userFacade.loadUserCurrencies();

    this.categoryFacade.loadCategories();
  }

  onCancel(): void {
    this.routerService.navigateByUrl('');
  }

  private getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
