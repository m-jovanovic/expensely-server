import { Component, OnInit } from '@angular/core';
import { AuthenticationFacade } from '@expensely/core/store';

@Component({
  selector: 'exp-account-dropdown',
  templateUrl: './account-dropdown.component.html',
  styleUrls: ['./account-dropdown.component.scss']
})
export class AccountDropdownComponent implements OnInit {
  isDropdownMenuOpen = false;

  constructor(private authenticationFacade: AuthenticationFacade) {}

  ngOnInit(): void {}

  toggleDropdownMenu(): void {
    this.isDropdownMenuOpen = !this.isDropdownMenuOpen;
  }

  logout(): void {
    this.authenticationFacade.logout();
  }
}
