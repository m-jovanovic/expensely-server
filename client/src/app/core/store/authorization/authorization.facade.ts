import { Injectable } from '@angular/core';

import { Permission } from '../../contracts/authentication/permission.enum';
import { AuthenticationFacade } from '../authentication';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationFacade {
  constructor(private authenticationFacade: AuthenticationFacade) {}

  hasPermission(permission: Permission): boolean {
    return this.authenticationFacade.userPermissions.includes(permission);
  }
}
