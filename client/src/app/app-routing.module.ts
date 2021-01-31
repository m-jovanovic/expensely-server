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
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
      },
      {
        path: 'transactions',
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
      },
      {
        path: 'budgets',
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
      },
      {
        path: 'subscription',
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
      },
      {
        path: 'account',
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
      },
      {
        path: 'settings',
        loadChildren: () => import('./modules').then((m) => m.DashboardModule),
        canLoad: [AuthenticationGuard]
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
