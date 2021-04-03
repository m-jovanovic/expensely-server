export class UpdateTransactionRequest {
  constructor(public description: string, category: number, amount: number, currency: number, occurredOn: Date) {}
}
