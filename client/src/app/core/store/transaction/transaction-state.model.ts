import { TransactionResponse } from '../../contracts/transactions/transaction-response';
import { TransactionDetailsResponse } from '../../contracts/transactions/transaction-details-response';

export interface TransactionStateModel {
  transactionId: string;
  transaction: TransactionResponse;
  transactionDetails: TransactionDetailsResponse;
  isLoading: boolean;
  error: boolean;
}

export const initialState: TransactionStateModel = {
  transactionId: '',
  transaction: null,
  transactionDetails: null,
  isLoading: false,
  error: false
};
