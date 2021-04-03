import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateService {
  getCurrentDateString(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
