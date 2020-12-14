using System.Collections.Generic;
using Expensely.Common.Messaging;
using Expensely.Contracts.Currencies;

namespace Expensely.Application.Queries.Currencies.GetCurrencies
{
    /// <summary>
    /// Represents the query for getting a collection of all supported currencies.
    /// </summary>
    public sealed class GetCurrenciesQuery : IQuery<IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
