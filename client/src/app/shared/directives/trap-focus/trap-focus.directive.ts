import { AfterViewInit, ElementRef, Directive, OnDestroy } from '@angular/core';

@Directive({
  selector: '[trapFocus]'
})
export class TrapFocusDirective implements AfterViewInit, OnDestroy {
  private htmlElement: HTMLElement;
  private firstFocusElement: HTMLElement;
  private lastFocusElement: HTMLElement;
  private boundKeydownEventHandler: EventListenerOrEventListenerObject;

  constructor(private element: ElementRef) {}

  ngAfterViewInit() {
    this.setHtmlElements();

    this.trapFocus();
  }

  private setHtmlElements() {
    this.htmlElement = this.element.nativeElement as HTMLElement;

    const allFocusElements = this.htmlElement.querySelectorAll(
      'a[href], button, textarea, input[type="text"], input[type="number"], input[type="date"], input[type="radio"], input[type="checkbox"], select'
    );

    const enabledFocusElements = Array.from(allFocusElements)
      .filter((el: any) => !el.disabled)
      .map((x) => x as HTMLElement);

    this.firstFocusElement = enabledFocusElements[0];

    this.lastFocusElement = enabledFocusElements[enabledFocusElements.length - 1];
  }

  ngOnDestroy(): void {
    this.htmlElement.removeEventListener('keydown', this.boundKeydownEventHandler, false);
  }

  private trapFocus() {
    this.boundKeydownEventHandler = this.handleKeydownEvent.bind(this);

    this.htmlElement.addEventListener('keydown', this.boundKeydownEventHandler, false);
  }

  private handleKeydownEvent(event: KeyboardEvent): void {
    const tabPressed = event.key === 'Tab';

    if (!tabPressed) {
      return;
    }

    if (event.shiftKey && document.activeElement === this.firstFocusElement) {
      this.lastFocusElement.focus();

      event.preventDefault();
    }

    if (!event.shiftKey && document.activeElement === this.lastFocusElement) {
      this.firstFocusElement.focus();

      event.preventDefault();
    }
  }
}
