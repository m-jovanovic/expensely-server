import { Component, ElementRef, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { ClickDetectionService } from '@expensely/shared/services';
import { AuthenticationFacade } from '../../store';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'exp-account-dropdown',
  templateUrl: './account-dropdown.component.html',
  styleUrls: ['./account-dropdown.component.scss']
})
export class AccountDropdownComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  userInitials: string;
  isDropdownMenuOpen = false;

  constructor(
    private elementRef: ElementRef,
    private authenticationFacade: AuthenticationFacade,
    private clickDetectionService: ClickDetectionService
  ) {}

  ngOnInit(): void {
    this.userInitials = this.authenticationFacade.userInitials;

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

  toggleDropdownMenu(): void {
    this.isDropdownMenuOpen = !this.isDropdownMenuOpen;
  }

  logout(): void {
    this.authenticationFacade.logout();
  }
}
