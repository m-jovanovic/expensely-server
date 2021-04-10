import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { RouterService, TransactionDetailsResponse, TransactionFacade } from '@expensely/core';

@Component({
  selector: 'exp-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss']
})
export class TransactionDetailsComponent implements OnInit {
  transaction$: Observable<TransactionDetailsResponse>;
  isLoading$: Observable<boolean>;

  constructor(private route: ActivatedRoute, private transactionFacade: TransactionFacade, private routerService: RouterService) {}

  ngOnInit(): void {
    this.transaction$ = this.transactionFacade.transactionDetails$;

    this.isLoading$ = this.transactionFacade.isLoading$;

    const transactionId = this.route.snapshot.paramMap.get('id');

    this.transactionFacade.getTransactionDetails(transactionId);
  }

  deleteTransaction(transactionId: string): void {
    this.transactionFacade.deleteTransaction(transactionId).subscribe(() => this.routerService.navigate(['/transactions']));
  }

  async updateTransaction(transactionId: string): Promise<boolean> {
    return await this.routerService.navigate(['transactions', transactionId, 'update']);
  }
}
