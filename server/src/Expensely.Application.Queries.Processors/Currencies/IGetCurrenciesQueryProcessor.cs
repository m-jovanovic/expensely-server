using System.Collections.Generic;
using Expensely.Application.Queries.Currencies;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Contracts.Currencies;

namespace Expensely.Application.Queries.Processors.Currencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrenciesQueryProcessor : IQueryProcessor<GetCurrenciesQuery, IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
