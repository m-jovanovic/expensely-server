import { ErrorItem as ApiError } from './api-error';
import { ErrorCodes } from './error-codes.enum';

export class ApiErrorResponse {
  constructor(public errors: ApiError[]) {}

  hasError(errorCode: ErrorCodes): boolean {
    return this.errors.some((e) => e.code === errorCode);
  }
}
