using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Queries.Processors.Users;
using Expensely.Application.Queries.Users;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Persistence.QueryProcessors.Users
{
    /// <summary>
    /// Represents the <see cref="GetUserCurrenciesQuery"/> processor.
    /// </summary>
    public sealed class GetUserCurrenciesQueryProcessor : IGetUserCurrenciesQueryProcessor
    {
        /// <inheritdoc />
        public Task<Maybe<IReadOnlyCollection<UserCurrencyResponse>>> Process(
            GetUserCurrenciesQuery query,
            CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();
    }
}
