import { CategoryResponse } from '../transactions';

export interface BudgetResponse {
  id: string;
  name: string;
  amount: number;
  currency: number;
  formattedAmount: string;
  categories: CategoryResponse[];
  startDate: string;
  endDate: string;
}
