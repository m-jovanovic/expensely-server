import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { LanguageFacade } from '../../../core/store';
import { LanguageModel, availableLanguages } from './language.model';

@Component({
  selector: 'exp-language-picker',
  templateUrl: './language-picker.component.html',
  styleUrls: ['./language-picker.component.scss']
})
export class LanguagePickerComponent implements OnInit {
  selectedLanguage$: Observable<LanguageModel>;
  languages: LanguageModel[] = availableLanguages;
  isOpen: boolean = false;

  constructor(private languageFacade: LanguageFacade) {}

  ngOnInit(): void {
    this.selectedLanguage$ = this.languageFacade.currentLanguage$.pipe(
      map((currentLanguage: string) => {
        return this.findLanguage(currentLanguage);
      })
    );
  }

  toggleLanguagePicker(): void {
    this.isOpen = !this.isOpen;
  }

  changeLanguage(code: string): void {
    this.isOpen = false;

    if (this.languageFacade.currentLanguage == code) {
      return;
    }

    this.languageFacade.changeLanguage(code);
  }

  private findLanguage(code: string): LanguageModel {
    return this.languages.find((x) => x.code == code);
  }
}
