import { TransactionListItem } from '../../contracts/transactions/transaction-list-item';

export interface TransactionListStateModel {
  transactions: TransactionListItem[];
  cursor: string;
  isLoading: boolean;
  error: boolean;
}
