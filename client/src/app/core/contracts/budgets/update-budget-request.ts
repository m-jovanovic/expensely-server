export class UpdateBudgetRequest {
  constructor(public name: string, public amount: number, public currency: number, public startDate: Date, public endDate: Date) {}
}
