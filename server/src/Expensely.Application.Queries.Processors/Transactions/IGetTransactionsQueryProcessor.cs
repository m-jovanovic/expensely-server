﻿using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Application.Queries.Transactions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Processors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> processor interface.
    /// </summary>
    public interface IGetTransactionsQueryProcessor : IQueryProcessor<GetTransactionsQuery, Maybe<TransactionListResponse>>
    {
    }
}