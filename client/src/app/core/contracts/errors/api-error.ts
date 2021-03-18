import { ErrorCode } from './error-code.enum';

export interface ErrorItem {
  code: ErrorCode;
  message: string;
}
