import { Component, OnInit } from '@angular/core';

import { LanguageFacade } from '@expensely/core/store/language';

@Component({
  selector: 'exp-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Expensely';

  constructor(private languageFacade: LanguageFacade) {}

  ngOnInit(): void {
    this.languageFacade.setDefaultLanguage();
  }
}
