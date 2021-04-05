using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Transactions;
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
            Maybe<Transaction> maybeTransaction = await _session.LoadAsync<Transaction>(query.TransactionId, cancellationToken);

            if (maybeTransaction.HasNoValue || maybeTransaction.Value.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionResponse>.None;
            }

            Transaction transaction = maybeTransaction.Value;

            var transactionResponse = TransactionResponse.FromTransaction(transaction);

            return transactionResponse;
        }
    }
}
