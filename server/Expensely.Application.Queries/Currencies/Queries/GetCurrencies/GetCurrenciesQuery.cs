using System.Collections.Generic;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Contracts.Currencies;

namespace Expensely.Application.Queries.Currencies.Queries.GetCurrencies
{
    /// <summary>
    /// Represents the query for getting a collection of all supported currencies.
    /// </summary>
    public sealed class GetCurrenciesQuery : IQuery<IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
