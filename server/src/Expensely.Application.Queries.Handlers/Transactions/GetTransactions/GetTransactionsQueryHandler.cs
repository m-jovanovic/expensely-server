﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Transactions.GetTransactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> handler.
    /// </summary>
    internal sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, Maybe<TransactionListResponse>>
    {
        private readonly IGetTransactionsQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get transactions query processor.</param>
        public GetTransactionsQueryHandler(IGetTransactionsQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<TransactionListResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
