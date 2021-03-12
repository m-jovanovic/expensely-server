import { UserCurrencyResponse } from '../../contracts/users';

export interface UserStateModel {
  currencies: UserCurrencyResponse[];
  isLoading: boolean;
  error: boolean;
}

export const initialState: UserStateModel = {
  currencies: [],
  isLoading: false,
  error: false
};
