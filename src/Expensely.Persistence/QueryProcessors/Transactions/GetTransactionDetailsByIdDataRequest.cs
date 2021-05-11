using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Handlers.Transactions.GetTransactionDetailsById;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the data request for getting transaction details by identifier.
    /// </summary>
    internal sealed class GetTransactionDetailsByIdDataRequest : IGetTransactionDetailsByIdDataRequest
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionDetailsByIdDataRequest"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public GetTransactionDetailsByIdDataRequest(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<TransactionDetailsModel> GetAsync(
            GetTransactionDetailsByIdRequest request,
            CancellationToken cancellationToken = default)
        {
            TransactionDetailsModel transactionDetailsModel = await _session
                .Query<Transaction>()
                .Where(x => x.Id == request.TransactionId)
                .Select(x => new TransactionDetailsModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Description = x.Description.Value,
                    Category = x.Category.Name,
                    Amount = x.Money.Amount,
                    Currency = x.Money.Currency.Value,
                    OccurredOn = x.OccurredOn
                })
                .FirstOrDefaultAsync(cancellationToken);

            return transactionDetailsModel;
        }
    }
}
