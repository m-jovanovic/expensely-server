import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationGuard } from './core';
import { EmptyLayoutComponent, MainLayoutComponent } from './core/components';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'dashboard',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.DashboardModule)
      },
      {
        path: 'transactions',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.TransactionsModule)
      },
      {
        path: 'budgets',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.BudgetsModule)
      },
      {
        path: 'subscription',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.DashboardModule)
      },
      {
        path: 'account',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.AccountModule)
      },
      {
        path: 'settings',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.DashboardModule)
      }
    ]
  },
  {
    path: '',
    component: EmptyLayoutComponent,
    children: [
      {
        path: 'login',
        loadChildren: () => import('./modules').then((m) => m.LoginModule)
      },
      {
        path: 'register',
        loadChildren: () => import('./modules').then((m) => m.RegisterModule)
      },
      {
        path: 'setup',
        canLoad: [AuthenticationGuard],
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./modules').then((m) => m.SetupModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
