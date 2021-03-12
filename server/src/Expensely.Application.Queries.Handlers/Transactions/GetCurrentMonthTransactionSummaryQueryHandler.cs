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
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> handler.
    /// </summary>
    public sealed class GetCurrentMonthTransactionSummaryQueryHandler
        : IQueryHandler<GetCurrentMonthTransactionSummaryQuery, Maybe<TransactionSummaryResponse>>
    {
        private readonly IGetCurrentMonthTransactionSummaryQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthTransactionSummaryQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get current month transaction summary query processor.</param>
        public GetCurrentMonthTransactionSummaryQueryHandler(IGetCurrentMonthTransactionSummaryQueryProcessor processor) =>
            _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<TransactionSummaryResponse>> Handle(
            GetCurrentMonthTransactionSummaryQuery request,
            CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
