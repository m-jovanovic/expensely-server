import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export interface TransactionListStateModel {
  transactions: TransactionResponse[];
  cursor: string;
  isLoading: boolean;
  error: boolean;
}
