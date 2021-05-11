using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Handlers.Transactions.GetTransactionById;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the data request for getting a transaction by identifier..
    /// </summary>
    internal sealed class GetTransactionByIdDataRequest : IGetTransactionByIdDataRequest
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionByIdDataRequest"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public GetTransactionByIdDataRequest(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<TransactionModel> GetAsync(GetTransactionByIdRequest request, CancellationToken cancellationToken = default)
        {
            TransactionModel transaction = await _session
                .Query<Transaction>()
                .Where(x => x.Id == request.TransactionId)
                .Select(x => new TransactionModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Description = x.Description.Value,
                    Category = x.Category.Value,
                    Amount = x.Money.Amount,
                    Currency = x.Money.Currency.Value,
                    OccurredOn = x.OccurredOn,
                    TransactionType = x.TransactionType.Value
                })
                .FirstOrDefaultAsync(cancellationToken);

            return transaction;
        }
    }
}
