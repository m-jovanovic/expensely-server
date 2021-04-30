using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionDetailsByIdQuery"/> processor.
    /// </summary>
    internal sealed class GetTransactionDetailsByIdQueryProcessor : IGetTransactionDetailsByIdQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionDetailsByIdQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetTransactionDetailsByIdQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionDetailsResponse>> Process(
            GetTransactionDetailsByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _session
                .Query<Transaction>()
                .Where(x => x.Id == query.TransactionId)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.Description,
                    Category = new
                    {
                        x.Category.Name
                    },
                    x.Money,
                    x.OccurredOn
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (transaction is null || transaction.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionDetailsResponse>.None;
            }

            var transactionDetailsResponse = new TransactionDetailsResponse
            {
                Id = transaction.Id,
                Description = transaction.Description.Value,
                Category = transaction.Category.Name,
                FormattedAmount = transaction.Money.Format(),
                OccurredOn = transaction.OccurredOn
            };

            return transactionDetailsResponse;
        }
    }
}
