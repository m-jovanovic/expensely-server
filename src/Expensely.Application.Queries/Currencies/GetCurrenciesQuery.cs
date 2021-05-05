using System.Collections.Generic;
using Expensely.Application.Contracts.Currencies;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Currencies
{
    /// <summary>
    /// Represents the query for getting the collection of all supported currencies.
    /// </summary>
    public sealed class GetCurrenciesQuery : IQuery<IEnumerable<CurrencyResponse>>
    {
    }
}
