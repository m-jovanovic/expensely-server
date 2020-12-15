import { TransactionResponse } from '../../contracts/transaction/transaction-response';

export interface TransactionStateModel {
  transactions: TransactionResponse[];
  cursor: string;
  isLoading: boolean;
  error: boolean;
}
