using System.Collections.Generic;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Application.Queries.Users;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Users
{
    /// <summary>
    /// Represents the <see cref="GetUserCurrenciesQuery"/> processor interface.
    /// </summary>
    public interface IGetUserCurrenciesQueryProcessor
        : IQueryProcessor<GetUserCurrenciesQuery, Maybe<IReadOnlyCollection<UserCurrencyResponse>>>
    {
    }
}
