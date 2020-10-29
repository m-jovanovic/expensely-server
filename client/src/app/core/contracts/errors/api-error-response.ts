import { HttpErrorResponse } from '@angular/common/http';

import { ErrorItem as ApiError } from './api-error';
import { ErrorCodes } from './error-codes.enum';

export class ApiErrorResponse {
  private errors: ApiError[];

  constructor(httpError: HttpErrorResponse) {
    this.errors = httpError.error?.errors ?? [];
  }

  hasError(errorCode: ErrorCodes): boolean {
    return this.errors.some((e) => e.code === errorCode);
  }
}
