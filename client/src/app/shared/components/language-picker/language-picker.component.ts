import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
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
  isDropdownMenuOpen: boolean = false;

  @HostListener('document:click', ['$event'])
  click(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target) && this.isDropdownMenuOpen) {
      this.isDropdownMenuOpen = false;
    }
  }

  constructor(private elementRef: ElementRef, private languageFacade: LanguageFacade) {}

  ngOnInit(): void {
    this.selectedLanguage$ = this.languageFacade.currentLanguage$.pipe(
      map((currentLanguage: string) => {
        return this.findLanguage(currentLanguage);
      })
    );
  }

  toggleLanguagePicker(): void {
    this.isDropdownMenuOpen = !this.isDropdownMenuOpen;
  }

  changeLanguage(code: string): void {
    this.isDropdownMenuOpen = false;

    if (this.languageFacade.currentLanguage == code) {
      return;
    }

    this.languageFacade.changeLanguage(code);
  }

  private findLanguage(code: string): LanguageModel {
    return this.languages.find((x) => x.code == code);
  }
}
