import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export interface TransactionStateModel {
  transactionId: string;
  transaction: TransactionResponse;
  isLoading: boolean;
  error: boolean;
}
