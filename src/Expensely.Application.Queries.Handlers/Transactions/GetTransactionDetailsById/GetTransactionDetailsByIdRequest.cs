namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionDetailsById
{
    /// <summary>
    /// Represents the request for getting transaction details by identifier.
    /// </summary>
    public sealed record GetTransactionDetailsByIdRequest(string TransactionId);
}
