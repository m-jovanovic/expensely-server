using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Currencies;
using Expensely.Domain.Core;

namespace Expensely.Application.Currencies.Queries.GetCurrencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> handler.
    /// </summary>
    internal sealed class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IReadOnlyCollection<CurrencyResponse>>
    {
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<CurrencyResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = Currency.List.Select(x => new CurrencyResponse(x.Value, x.Name, x.Code)).ToList();

            return await Task.FromResult(currencies);
        }
    }
}
