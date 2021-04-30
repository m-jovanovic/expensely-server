using System.Collections.Generic;
using Expensely.Application.Contracts.Currencies;
using Expensely.Application.Queries.Currencies;
using Expensely.Application.Queries.Processors.Abstractions;

namespace Expensely.Application.Queries.Processors.Currencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrenciesQueryProcessor : IQueryProcessor<GetCurrenciesQuery, IReadOnlyCollection<CurrencyResponse>>
    {
    }
}
