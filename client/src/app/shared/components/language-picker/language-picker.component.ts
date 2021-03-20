import { Component, OnInit } from '@angular/core';

import { LanguageFacade } from '../../../core/store';
import { LanguageModel, availableLanguages } from './language.model';

@Component({
  selector: 'exp-language-picker',
  templateUrl: './language-picker.component.html',
  styleUrls: ['./language-picker.component.scss']
})
export class LanguagePickerComponent implements OnInit {
  // TODO: Make this an observable.
  selectedLanguage: LanguageModel;
  languages: LanguageModel[] = availableLanguages;
  isOpen: boolean = false;

  constructor(private languageFacade: LanguageFacade) {}

  ngOnInit(): void {
    this.selectedLanguage = this.findLanguage(this.languageFacade.currentLanguage);
  }

  toggleLanguagePicker(): void {
    this.isOpen = !this.isOpen;
  }

  changeLanguage(code: string): void {
    this.isOpen = false;

    if (this.selectedLanguage.code == code) {
      return;
    }

    this.selectedLanguage = this.findLanguage(code);

    this.languageFacade.changeLanguage(code);
  }

  private findLanguage(code: string): LanguageModel {
    return this.languages.find((x) => x.code == code);
  }
}
