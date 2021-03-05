import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { TransactionFacade, TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transactions',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss']
})
export class TransactionComponent implements OnInit {
  private readonly limit = 25;
  transactions$: Observable<TransactionResponse[]>;
  isLoading$: Observable<boolean>;
  error$: Observable<boolean>;

  constructor(private transactionFacade: TransactionFacade) {}

  ngOnInit(): void {
    this.transactions$ = this.transactionFacade.transactions$;
    this.isLoading$ = this.transactionFacade.isLoading$;
    this.error$ = this.transactionFacade.error$;

    this.transactionFacade.loadTransactions(this.limit);
  }
}
