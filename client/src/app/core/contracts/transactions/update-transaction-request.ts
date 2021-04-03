export class UpdateTransactionRequest {
  constructor(
    public description: string,
    public category: number,
    public amount: number,
    public currency: number,
    public occurredOn: Date
  ) {}
}
