using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Currencies.GetCurrencies;
using Expensely.Common.Abstractions.Messaging;
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
        public Task<IReadOnlyCollection<CurrencyResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<CurrencyResponse> currencies = Currency
                .List
                .Select(x => new CurrencyResponse(x.Value, x.Name, x.Code))
                .ToList();

            return Task.FromResult(currencies);
        }
    }
}
