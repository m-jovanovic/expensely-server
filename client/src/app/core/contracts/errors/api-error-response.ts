import { HttpErrorResponse } from '@angular/common/http';

import { ErrorItem as ApiError } from './api-error';
import { ErrorCode } from './error-code.enum';

export class ApiErrorResponse {
  private errors: ApiError[];

  constructor(httpError: HttpErrorResponse) {
    this.errors = httpError.error?.errors ?? [];
  }

  hasError(errorCode: ErrorCode): boolean {
    return this.errors.some((e) => e.code === errorCode);
  }

  hasErrors(): boolean {
    return this.errors.length !== 0;
  }
}
