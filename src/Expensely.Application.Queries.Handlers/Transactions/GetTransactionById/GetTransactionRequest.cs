namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionById
{
    /// <summary>
    /// Represents the request for getting a transaction.
    /// </summary>
    public sealed record GetTransactionRequest(string TransactionId);
}
