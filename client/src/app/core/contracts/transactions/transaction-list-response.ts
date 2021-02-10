import { TransactionResponse } from './transaction-response';

export interface TransactionListResponse {
  items: TransactionResponse[];
  cursor: string;
}
