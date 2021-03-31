export interface CreateBudgetRequest {
  userId: string;
  name: string;
  amount: number;
  currency: number;
  categories: number[];
  startDate: Date;
  endDate: Date;
}
