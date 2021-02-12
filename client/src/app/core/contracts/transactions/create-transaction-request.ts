export class CreateTransactionRequest {
  constructor(
    public userId: string,
    public description: string,
    public category: number,
    public amount: number,
    public currency: number,
    public occurredOn: Date,
    public transactionType: number
  ) {}
}
