export class UpdateExpenseRequest {
  constructor(public name: string, public amount: number, public currency: number, public occurredOn: Date, public description: string) {}
}
