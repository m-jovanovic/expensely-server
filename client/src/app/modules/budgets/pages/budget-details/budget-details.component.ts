import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';

import { BudgetFacade, BudgetDetailsResponse, RouterService } from '@expensely/core';
import { ConfirmationDialogService } from '@expensely/shared/services';

@Component({
  selector: 'exp-budget-details',
  templateUrl: './budget-details.component.html',
  styleUrls: ['./budget-details.component.scss']
})
export class BudgetDetailsComponent implements OnInit {
  budget$: Observable<BudgetDetailsResponse>;
  isLoading$: Observable<boolean>;

  constructor(
    private route: ActivatedRoute,
    private budgetFacade: BudgetFacade,
    private routerService: RouterService,
    private confirmationDialogService: ConfirmationDialogService,
    private translationService: TranslocoService
  ) {}

  ngOnInit(): void {
    this.budget$ = this.budgetFacade.budgetDetails$;

    this.isLoading$ = this.budgetFacade.isLoading$;

    const budgetId = this.route.snapshot.paramMap.get('id');

    this.budgetFacade.getBudgetDetails(budgetId);
  }

  deleteBudget(budgetId: string): void {
    this.confirmationDialogService.open({
      message: this.translationService.translate('budgets.delete.dialog.message'),
      cancelButtonText: this.translationService.translate('budgets.delete.dialog.cancelButtonText'),
      confirmButtonText: this.translationService.translate('budgets.delete.dialog.confirmButtonText')
    });

    this.confirmationDialogService.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.budgetFacade.deleteBudget(budgetId).subscribe(() => this.routerService.navigate(['/budgets']));
    });
  }

  async updateBudget(budgetId: string): Promise<boolean> {
    return await this.routerService.navigate(['budgets', budgetId, 'update']);
  }
}
