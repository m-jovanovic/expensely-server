using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionDetailsByIdQuery"/> handler.
    /// </summary>
    public sealed class GetTransactionDetailsByIdQueryHandler :
        IQueryHandler<GetTransactionDetailsByIdQuery, Maybe<TransactionDetailsResponse>>
    {
        private readonly IGetTransactionDetailsByIdQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionDetailsByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get transaction by identifier query processor.</param>
        public GetTransactionDetailsByIdQueryHandler(IGetTransactionDetailsByIdQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<TransactionDetailsResponse>> Handle(
            GetTransactionDetailsByIdQuery request,
            CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
