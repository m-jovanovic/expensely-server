using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Handlers.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> handler.
    /// </summary>
    public sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, TransactionListResponse>
    {
        private readonly IGetTransactionsQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get transactions query processor.</param>
        public GetTransactionsQueryHandler(IGetTransactionsQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<TransactionListResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
