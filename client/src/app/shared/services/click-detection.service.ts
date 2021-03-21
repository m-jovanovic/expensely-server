import { Injectable } from '@angular/core';
import { fromEvent, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClickDetectionService {
  private clicks$: Observable<MouseEvent>;

  constructor() {
    this.clicks$ = fromEvent(document, 'click').pipe(map((event: Event) => event as MouseEvent));
  }

  getClicks(): Observable<MouseEvent> {
    return this.clicks$;
  }
}
