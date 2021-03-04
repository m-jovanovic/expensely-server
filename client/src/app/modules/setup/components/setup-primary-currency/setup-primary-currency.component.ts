import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CurrencyResponse } from '@expensely/core';

@Component({
  selector: 'exp-setup-primary-currency',
  templateUrl: './setup-primary-currency.component.html',
  styleUrls: ['./setup-primary-currency.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SetupPrimaryCurrencyComponent implements OnInit {
  setupForm: FormGroup;
  submitted = false;
  requestSent = false;

  @Input()
  currencies: CurrencyResponse[];

  @Input()
  isLoading: boolean;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.setupForm = this.formBuilder.group({
      currency: ['', Validators.required]
    });
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

    this.setupForm.disable();
  }
}
