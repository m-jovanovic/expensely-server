import { Component } from '@angular/core';

import { LanguageFacade } from '../../../core/store';
import { LanguageModel, availableLanguages } from './language.model';

@Component({
  selector: 'exp-language-picker',
  templateUrl: './language-picker.component.html',
  styleUrls: ['./language-picker.component.scss']
})
export class LanguagePickerComponent {
  languages: LanguageModel[] = availableLanguages;

  constructor(private languageFacade: LanguageFacade) {}

  changeLanguage(language: string): void {
    this.languageFacade.changeLanguage(language);
  }
}
