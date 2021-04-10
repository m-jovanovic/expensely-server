export interface BudgetDetailsResponse {
  id: string;
  name: string;
  amount: string;
  remainingAmount: string;
  usedPercentage: number;
  categories: string[];
  startDate: string;
  endDate: string;
}
