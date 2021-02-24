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
    /// Represents the <see cref="GetTransactionByIdQuery"/> handler.
    /// </summary>
    public sealed class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, Maybe<TransactionResponse>>
    {
        private readonly IGetTransactionByIdQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get transaction by identifier query processor.</param>
        public GetTransactionByIdQueryHandler(IGetTransactionByIdQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<TransactionResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
