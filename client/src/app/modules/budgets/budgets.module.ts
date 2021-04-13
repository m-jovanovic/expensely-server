import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { TRANSLOCO_SCOPE } from '@ngneat/transloco';

import { SharedModule } from '@expensely/shared';
import { BudgetState, CategoryState, UserState } from '@expensely/core';
import { BudgetsRoutingModule } from './budgets-routing.module';
import { SelectBudgetCategoryComponent } from './components';
import { CreateBudgetComponent, UpdateBudgetComponent, BudgetDetailsComponent } from './pages';

export const budgetsTranslationsLoader = ['en', 'sr'].reduce((acc, language) => {
  acc[language] = () => import(`./i18n/${language}.json`);

  return acc;
}, {});

@NgModule({
  declarations: [CreateBudgetComponent, UpdateBudgetComponent, BudgetDetailsComponent, SelectBudgetCategoryComponent],
  imports: [SharedModule, ReactiveFormsModule, BudgetsRoutingModule, NgxsModule.forFeature([BudgetState, CategoryState, UserState])],
  providers: [
    {
      provide: TRANSLOCO_SCOPE,
      useValue: {
        scope: 'budgets',
        loader: budgetsTranslationsLoader
      }
    }
  ]
})
export class BudgetsModule {}
