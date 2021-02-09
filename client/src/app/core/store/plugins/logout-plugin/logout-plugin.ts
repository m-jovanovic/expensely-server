import { getActionTypeFromInstance, NgxsNextPluginFn, NgxsPlugin } from '@ngxs/store';

import { Logout } from '../../authentication/authentication.actions';

export class LogoutPlugin implements NgxsPlugin {
  handle(state: any, action: any, next: NgxsNextPluginFn) {
    if (getActionTypeFromInstance(action) === Logout.type) {
      state = {};
    }

    return next(state, action);
  }
}
