import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { RouterService, TransactionListFacade, TransactionResponse } from '@expensely/core';

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

  constructor(private transactionFacade: TransactionListFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.transactions$ = this.transactionFacade.transactions$;
    this.isLoading$ = this.transactionFacade.isLoading$;
    this.error$ = this.transactionFacade.error$;

    this.transactionFacade.loadTransactions(this.limit);
  }

  async selectTransaction(transactionId: string): Promise<boolean> {
    return await this.routerService.navigate(['/transactions', transactionId]);
  }
}
