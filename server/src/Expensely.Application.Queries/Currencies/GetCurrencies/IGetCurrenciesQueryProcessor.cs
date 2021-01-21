using System.Collections.Generic;
using Expensely.Application.Queries.Abstractions;
using Expensely.Contracts.Currencies;

namespace Expensely.Application.Queries.Currencies.GetCurrencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrenciesQueryProcessor : IQueryProcessor<GetCurrenciesQuery, IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
