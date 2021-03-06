import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { TransactionFacade, TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss']
})
export class TransactionDetailsComponent implements OnInit {
  transaction$: Observable<TransactionResponse>;
  isLoading$: Observable<boolean>;

  constructor(private route: ActivatedRoute, private transactionFacade: TransactionFacade) {}

  ngOnInit(): void {
    this.transaction$ = this.transactionFacade.transaction$;

    this.isLoading$ = this.transactionFacade.isLoading$;

    const transactionId = this.route.snapshot.paramMap.get('id');

    this.transactionFacade.getTransactionById(transactionId);
  }
}
