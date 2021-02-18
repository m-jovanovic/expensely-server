using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Abstractions.Constants;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Modules.Transactions;
using Expensely.Persistence.Indexes.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> processor.
    /// </summary>
    public sealed class GetTransactionsQueryProcessor : IGetTransactionsQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetTransactionsQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionListResponse>> Process(GetTransactionsQuery query, CancellationToken cancellationToken = default)
        {
            if (query.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionListResponse>.None;
            }

            var transactions = await _session
                .Query<Transaction, Transactions_ByUserIdAndOccurredOnAndCreatedOnAndTransactionType>()
                .Where(x =>
                    x.UserId == query.UserId &&
                    (x.OccurredOn < query.OccurredOn ||
                     x.OccurredOn == query.OccurredOn && x.CreatedOnUtc <= query.CreatedOnUtc))
                .OrderByDescending(x => x.OccurredOn)
                .ThenByDescending(x => x.CreatedOnUtc)
                .Take(query.Limit)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Category,
                    x.Money,
                    x.OccurredOn,
                    x.CreatedOnUtc
                })
                .ToArrayAsync(cancellationToken);

            TransactionResponse[] transactionResponses = transactions
                .Select(x => new TransactionResponse(x.Id, x.Description, x.Category, x.Money, x.OccurredOn))
                .ToArray();

            if (transactionResponses.Length < query.Limit)
            {
                return new TransactionListResponse(transactionResponses);
            }

            var lastTransaction = transactions[^1];

            string cursor = Cursor.Create(
                lastTransaction.OccurredOn.ToString(DateTimeFormats.Date, CultureInfo.InvariantCulture),
                lastTransaction.CreatedOnUtc.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            return new TransactionListResponse(transactionResponses[..^1], cursor);
        }
    }
}
