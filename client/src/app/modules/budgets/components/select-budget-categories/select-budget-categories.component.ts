import {
  AfterViewChecked,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild
} from '@angular/core';

import { CategoryResponse } from '@expensely/core';

@Component({
  selector: 'exp-select-budget-categories',
  templateUrl: './select-budget-categories.component.html',
  styleUrls: ['./select-budget-categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SelectBudgetCategoryComponent implements AfterViewChecked {
  private selectedCategoriesChecked = false;
  private categorySelectChecked = false;

  @ViewChild('categorySelect')
  categorySelect: ElementRef;

  @Input()
  categories: CategoryResponse[];

  @Input()
  selectedCategoryIds: number[] = [];

  @Output()
  selectedCategoriesChangedEvent = new EventEmitter<number[]>();

  selectedCategories: CategoryResponse[] = [];

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngAfterViewChecked(): void {
    this.setSelectedCategoriesIfNotChecked();

    this.clearCategorySelectIfNotChecked();
  }

  selectCategory(categoryId: number): void {
    const selectedCategory = this.categories.find((category) => category.id == categoryId);

    this.selectedCategories = [...this.selectedCategories, selectedCategory];

    this.categories = this.categories.filter((category) => category.id != categoryId);

    this.processChangeEvent();
  }

  unselectCategory(categoryToRemove: CategoryResponse): void {
    this.selectedCategories = this.selectedCategories.filter((category) => category != categoryToRemove);

    this.categories = [...this.categories, categoryToRemove].sort((category1, category2) => category1.id - category2.id);

    this.processChangeEvent();
  }

  private processChangeEvent() {
    this.changeDetectorRef.detectChanges();

    this.clearCategorySelect();

    this.publishChangeEvent();
  }

  private publishChangeEvent() {
    this.selectedCategoriesChangedEvent.emit(this.selectedCategories.map((category) => category.id));
  }

  private clearCategorySelectIfNotChecked(): void {
    if (this.categorySelectChecked) {
      return;
    }

    this.clearCategorySelect();

    this.categorySelectChecked = true;

    this.changeDetectorRef.detectChanges();
  }

  private clearCategorySelect(): void {
    const selectElement = this.categorySelect?.nativeElement as HTMLSelectElement;

    if (selectElement) {
      selectElement.value = '';
    }
  }

  private setSelectedCategoriesIfNotChecked() {
    if (this.selectedCategoriesChecked) {
      return;
    }

    this.setSelectedCategories();

    this.selectedCategoriesChecked = true;

    this.changeDetectorRef.detectChanges();
  }

  private setSelectedCategories() {
    if (this.selectedCategoryIds?.length === 0) {
      return;
    }

    this.selectedCategories = this.categories.filter((category) => this.selectedCategoryIds.includes(category.id));

    this.categories = [...this.categories.filter((category) => !this.selectedCategories.includes(category))];

    this.selectedCategoriesChecked = true;
  }
}
