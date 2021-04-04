import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { RouterService, TransactionFacade, TransactionResponse } from '@expensely/core';

@Component({
  selector: 'exp-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss']
})
export class TransactionDetailsComponent implements OnInit {
  transaction$: Observable<TransactionResponse>;
  isLoading$: Observable<boolean>;

  constructor(private route: ActivatedRoute, private transactionFacade: TransactionFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.transaction$ = this.transactionFacade.transaction$;

    this.isLoading$ = this.transactionFacade.isLoading$;

    const transactionId = this.route.snapshot.paramMap.get('id');

    this.transactionFacade.getTransaction(transactionId);
  }

  deleteTransaction(transactionId: string): void {
    this.transactionFacade.deleteTransaction(transactionId).subscribe(() => this.routerService.navigate(['/transactions']));
  }

  async return(): Promise<boolean> {
    return await this.routerService.navigate(['/transactions']);
  }

  async update(transactionId: string): Promise<boolean> {
    return await this.routerService.navigate(['transactions', transactionId, 'update']);
  }
}
