using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Currencies;
using Expensely.Application.Queries.Processors.Currencies;
using Expensely.Contracts.Currencies;
using Expensely.Domain.Modules.Shared;

namespace Expensely.Persistence.QueryProcessors.Currencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> processor.
    /// </summary>
    public sealed class GetCurrenciesQueryProcessor : IGetCurrenciesQueryProcessor
    {
        /// <inheritdoc />
        public Task<IReadOnlyCollection<CurrencyResponse>> Process(GetCurrenciesQuery query, CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<CurrencyResponse> categories = Currency
                .List
                .Select(x => new CurrencyResponse
                {
                    Id = x.Value,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();

            return Task.FromResult(categories);
        }
    }
}
