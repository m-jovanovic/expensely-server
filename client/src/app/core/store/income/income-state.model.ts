import { IncomeResponse } from '../../contracts/incomes/income-response';

export interface IncomeStateModel {
  isLoading: boolean;
  incomes: IncomeResponse[];
}
