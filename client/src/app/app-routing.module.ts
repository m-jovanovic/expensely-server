import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmptyLayoutComponent } from './core/components';

const routes: Routes = [
  {
		path: '',
		redirectTo: 'dashboard',
		pathMatch: 'full'
	},
  {
    path: '',
    component: EmptyLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./modules/authentication').then((m) => m.AuthenticationModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
