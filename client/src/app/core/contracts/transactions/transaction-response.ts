export interface TransactionResponse {
  id: string;
  description: string;
  category: number;
  amount: number;
  currency: number;
  occurredOn: string;
  transactionType: number;
}
