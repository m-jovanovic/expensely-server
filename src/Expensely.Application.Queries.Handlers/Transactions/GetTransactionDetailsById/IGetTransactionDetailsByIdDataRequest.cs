using Expensely.Application.Queries.Handlers.Abstractions;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionDetailsById
{
    /// <summary>
    /// Represents the data request interface for getting transaction details by identifier.
    /// </summary>
    public interface IGetTransactionDetailsByIdDataRequest : IDataRequest<GetTransactionDetailsByIdRequest, TransactionDetailsModel>
    {
    }
}
