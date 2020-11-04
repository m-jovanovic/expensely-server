import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { TokenInfo } from '@expensely/core/contracts';
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
    this.userInitials = this.parseUserInitials(this.authenticationFacade.tokenInfo);
  }

  toggleDropdownMenu(): void {
    this.isDropdownMenuOpen = !this.isDropdownMenuOpen;
  }

  logout(): void {
    this.authenticationFacade.logout();
  }

  private parseUserInitials(tokenInfo: TokenInfo): string {
    const nameParts = tokenInfo.fullName.split(' ');

    if (nameParts.length === 0 || nameParts.some((x) => x.length === 0)) {
      return 'N/A';
    }

    return `${nameParts[0][0]}${nameParts[1][0]}`;
  }
}
