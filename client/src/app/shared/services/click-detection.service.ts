import { Injectable, OnInit } from '@angular/core';
import { fromEvent, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClickDetectionService implements OnInit {
  private clicks$: Observable<MouseEvent>;

  ngOnInit(): void {
    this.clicks$ = fromEvent(document, 'click').pipe(map((event: Event) => event as MouseEvent));
  }

  getClicks(): Observable<MouseEvent> {
    return this.clicks$;
  }
}
