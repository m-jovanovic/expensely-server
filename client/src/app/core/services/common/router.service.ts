import { Injectable } from '@angular/core';
import { NavigationExtras, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  constructor(private store: Store) {}

  navigate(paths: any[], queryParams?: Params | undefined, extras?: NavigationExtras | undefined): Observable<any> {
    return this.store.dispatch(new Navigate(paths, queryParams, extras));
  }
}
