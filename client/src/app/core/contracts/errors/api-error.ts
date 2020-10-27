import { ErrorCodes } from './error-codes.enum';

export interface ErrorItem {
  code: ErrorCodes;
  message: string;
}
