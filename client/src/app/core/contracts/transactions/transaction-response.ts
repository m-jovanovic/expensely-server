import { CategoryResponse } from './category-response';

export interface TransactionResponse {
  id: string;
  description: string;
  category: CategoryResponse;
  formattedAmount: string;
  amount: number;
  currency: number;
  occurredOn: string;
  transactionType: number;
}
