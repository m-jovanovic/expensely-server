import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterService } from '@expensely/core';

import { CategoryResponse } from '@expensely/core/contracts/transactions/category-response';
import { CurrencyResponse } from '@expensely/core/contracts/transactions/currency-response';

@Component({
  selector: 'exp-create-transaction-details',
  templateUrl: './create-transaction-details.component.html',
  styleUrls: ['./create-transaction-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateTransactionDetailsComponent implements OnInit {
  createTransactionForm: FormGroup;

  @Input()
  categoriesIsLoading: boolean;

  @Input()
  categories: CategoryResponse[];

  @Input()
  currenciesIsLoading: boolean;

  @Input()
  currencies: CurrencyResponse[];

  constructor(private formBuilder: FormBuilder, private routerService: RouterService) {}

  ngOnInit() {
    this.createTransactionForm = this.formBuilder.group({
      description: ['', Validators.required],
      category: ['', Validators.required],
      amount: ['0.00', Validators.required],
      currency: ['', Validators.required],
      occurredOn: [this.getCurrentDateString(), Validators.required]
    });
  }

  onCancel(): void {
    this.routerService.navigate(['']);
  }

  onSubmit(): void {}

  private getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
