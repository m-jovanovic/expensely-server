namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionById
{
    /// <summary>
    /// Represents the request for getting a transaction by identifier.
    /// </summary>
    public sealed record GetTransactionByIdRequest(string TransactionId);
}
