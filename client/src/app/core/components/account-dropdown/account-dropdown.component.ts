import { Component, ElementRef, HostListener, OnInit } from '@angular/core';

import { AuthenticationFacade } from '@expensely/core/store';

@Component({
  selector: 'exp-account-dropdown',
  templateUrl: './account-dropdown.component.html',
  styleUrls: ['./account-dropdown.component.scss']
})
export class AccountDropdownComponent implements OnInit {
  userInitials: string;
  isDropdownMenuOpen = false;

  @HostListener('document:click', ['$event'])
  click(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target) && this.isDropdownMenuOpen) {
      this.isDropdownMenuOpen = false;
    }
  }

  constructor(private elementRef: ElementRef, private authenticationFacade: AuthenticationFacade) {}

  ngOnInit(): void {
    this.userInitials = this.authenticationFacade.userInitials;
  }

  toggleDropdownMenu(): void {
    this.isDropdownMenuOpen = !this.isDropdownMenuOpen;
  }

  logout(): void {
    this.authenticationFacade.logout();
  }
}
