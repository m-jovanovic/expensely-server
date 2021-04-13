import {
  AfterViewChecked,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
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
export class SelectBudgetCategoryComponent implements OnInit, AfterViewChecked {
  private firstCheckPassed = false;

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

  ngOnInit(): void {}

  ngAfterViewChecked(): void {
    this.setSelectedCategoriesOnce();

    this.clearCategorySelect();
  }

  selectCategory(categoryId: number): void {
    const selectedCategory = this.categories.find((category) => category.id == categoryId);

    this.selectedCategories = [...this.selectedCategories, selectedCategory];

    this.categories = this.categories.filter((category) => category.id != categoryId);

    this.changeDetectorRef.detectChanges();

    this.clearCategorySelect();

    this.publishChangeEvent();
  }

  unselectCategory(categoryToRemove: CategoryResponse): void {
    this.selectedCategories = this.selectedCategories.filter((category) => category != categoryToRemove);

    this.categories = [...this.categories, categoryToRemove].sort((category1, category2) => category1.id - category2.id);

    this.publishChangeEvent();
  }

  private publishChangeEvent() {
    this.selectedCategoriesChangedEvent.emit(this.selectedCategories.map((category) => category.id));
  }

  private clearCategorySelect(): void {
    const selectElement = this.categorySelect?.nativeElement as HTMLSelectElement;

    if (selectElement) {
      selectElement.value = '';
    }
  }

  private setSelectedCategoriesOnce() {
    if (this.selectedCategoryIds.length === 0 || this.firstCheckPassed) {
      return;
    }

    this.selectedCategories = this.categories.filter((category) => this.selectedCategoryIds.includes(category.id));

    this.categories = this.categories.filter((category) => !this.selectedCategories.includes(category));

    this.changeDetectorRef.detectChanges();

    this.firstCheckPassed = true;
  }
}
