import { TransactionResponse } from '../../contracts/transactions/transaction-response';

export interface TransactionStateModel {
  transactionId: string;
  transaction: TransactionResponse;
  isLoading: boolean;
  error: boolean;
}

export const initialState: TransactionStateModel = {
  transactionId: '',
  transaction: null,
  isLoading: false,
  error: false
};
