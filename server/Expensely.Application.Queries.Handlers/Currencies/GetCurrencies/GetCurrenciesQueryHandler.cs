using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Currencies.Queries.GetCurrencies;
using Expensely.Application.Queries.Handlers.Abstractions;
using Expensely.Common.Messaging;
using Expensely.Contracts.Currencies;
using Expensely.Domain.Core;

namespace Expensely.Application.Queries.Handlers.Currencies.GetCurrencies
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
