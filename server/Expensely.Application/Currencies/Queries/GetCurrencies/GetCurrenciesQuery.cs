using System.Collections.Generic;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Currencies;

namespace Expensely.Application.Currencies.Queries.GetCurrencies
{
    /// <summary>
    /// Represents the query for getting a collection of all supported currencies.
    /// </summary>
    public sealed class GetCurrenciesQuery : IQuery<IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
