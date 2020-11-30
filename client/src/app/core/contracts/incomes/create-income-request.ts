export class CreateIncomeRequest {
  constructor(
    public userId: string,
    public name: string,
    public amount: number,
    public currency: number,
    public occurredOn: Date,
    public description: string
  ) {}
}
