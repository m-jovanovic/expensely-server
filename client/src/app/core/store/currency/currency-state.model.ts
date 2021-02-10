import { CurrencyResponse } from '../../contracts/transactions/currency-response';

export interface CurrencyStateModel {
  currencies: CurrencyResponse[];
  isLoading: boolean;
}
