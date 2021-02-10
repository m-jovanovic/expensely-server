import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export interface TransactionStateModel {
  transactions: TransactionResponse[];
  cursor: string;
  isLoading: boolean;
  error: boolean;
}
