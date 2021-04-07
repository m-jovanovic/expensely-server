import { Component, ElementRef, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { ClickDetectionService } from '../../services/click-detection.service';
import { LanguageFacade } from '../../../core/store';
import { LanguageModel, availableLanguages } from './language.model';

@Component({
  selector: 'exp-language-picker',
  templateUrl: './language-picker.component.html',
  styleUrls: ['./language-picker.component.scss']
})
export class LanguagePickerComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  selectedLanguage$: Observable<LanguageModel>;
  languages: LanguageModel[] = availableLanguages;
  isDropdownMenuOpen: boolean = false;

  constructor(
    private elementRef: ElementRef,
    private languageFacade: LanguageFacade,
    private clickDetectionService: ClickDetectionService
  ) {}

  ngOnInit(): void {
    this.selectedLanguage$ = this.languageFacade.currentLanguage$.pipe(
      map((currentLanguage: string) => {
        return this.findLanguage(currentLanguage);
      })
    );

    this.subscription = this.clickDetectionService
      .getClicks()
      .pipe(
        tap((mouseEvent: MouseEvent) => {
          if (!this.elementRef.nativeElement.contains(mouseEvent.target) && this.isDropdownMenuOpen) {
            this.isDropdownMenuOpen = false;
          }
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
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
    return this.languages.find((language) => language.code == code);
  }
}
