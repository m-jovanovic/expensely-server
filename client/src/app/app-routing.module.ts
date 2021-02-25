import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from './core';
import { MainLayoutComponent, EmptyLayoutComponent } from './core/components';

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
        loadChildren: () => import('./modules').then((m) => m.DashboardModule)
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
        loadChildren: () => import('./modules').then((m) => m.DashboardModule)
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
        path: '',
        loadChildren: () => import('./modules').then((m) => m.AuthenticationModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
