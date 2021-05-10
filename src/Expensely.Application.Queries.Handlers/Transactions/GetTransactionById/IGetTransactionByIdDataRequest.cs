using Expensely.Application.Queries.Handlers.Abstractions;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionById
{
    /// <summary>
    /// Represents the data request for getting a transaction by identifier interface.
    /// </summary>
    public interface IGetTransactionByIdDataRequest : IDataRequest<GetTransactionRequest, TransactionModel>
    {
    }
}
