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
    /// Represents the <see cref="GetTransactionByIdQuery"/> processor.
    /// </summary>
    internal sealed class GetTransactionByIdQueryProcessor : IGetTransactionByIdQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionByIdQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetTransactionByIdQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionResponse>> Process(GetTransactionByIdQuery query, CancellationToken cancellationToken = default)
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
                        x.Category.Value
                    },
                    Money = new
                    {
                        x.Money.Amount,
                        Currency = x.Money.Currency.Value
                    },
                    x.OccurredOn,
                    TransactionType = new
                    {
                        x.TransactionType.Value
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (transaction is null || transaction.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionResponse>.None;
            }

            var transactionResponse = new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description.Value,
                Category = transaction.Category.Value,
                Amount = transaction.Money.Amount,
                Currency = transaction.Money.Currency,
                OccurredOn = transaction.OccurredOn,
                TransactionType = transaction.TransactionType.Value
            };

            return transactionResponse;
        }
    }
}
