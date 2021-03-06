import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export interface TransactionStateModel {
  transaction: TransactionResponse;
  isLoading: boolean;
  error: boolean;
}
