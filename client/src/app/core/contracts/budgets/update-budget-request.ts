export class UpdateBudgetRequest {
  constructor(
    public readonly name: string,
    public readonly amount: number,
    public readonly currency: number,
    public readonly categories: number[],
    public readonly startDate: Date,
    public readonly endDate: Date
  ) {}
}
