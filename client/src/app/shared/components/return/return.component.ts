import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'exp-return',
  templateUrl: './return.component.html',
  styleUrls: ['./return.component.scss']
})
export class ReturnComponent {
  @Input()
  returnUrl: string;

  constructor() {}
}
