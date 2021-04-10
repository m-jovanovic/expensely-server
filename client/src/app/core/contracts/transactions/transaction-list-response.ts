import { TransactionListItem } from './transaction-list-item';

export interface TransactionListResponse {
  items: TransactionListItem[];
  cursor: string;
}
