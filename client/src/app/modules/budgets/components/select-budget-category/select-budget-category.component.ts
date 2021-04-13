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
  selector: 'exp-select-budget-category',
  templateUrl: './select-budget-category.component.html',
  styleUrls: ['./select-budget-category.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SelectBudgetCategoryComponent implements OnInit, AfterViewChecked {
  @ViewChild('categorySelect')
  categorySelect: ElementRef;

  @Input()
  categories: CategoryResponse[];

  @Input()
  selectedCategoryIds: number[];

  @Output()
  selectedCategoriesChangedEvent = new EventEmitter<number[]>();

  selectedCategories: CategoryResponse[] = [];

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit(): void {}

  ngAfterViewChecked(): void {
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
}
