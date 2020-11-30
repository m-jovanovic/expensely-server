export class UpdateIncomeRequest {
  constructor(public name: string, public amount: number, public currency: number, public occurredOn: Date, public description: string) {}
}
