export interface TransactionResponse {
  id: string;
  description: string;
  category: string;
  categoryValue: number;
  formattedAmount: string;
  amount: number;
  currency: number;
  occurredOn: string;
  transactionType: number;
}
