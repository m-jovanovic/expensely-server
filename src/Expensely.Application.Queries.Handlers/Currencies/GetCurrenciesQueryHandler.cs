using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Currencies;
using Expensely.Application.Queries.Currencies;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Modules.Common;

namespace Expensely.Application.Queries.Handlers.Currencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> handler.
    /// </summary>
    public sealed class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IEnumerable<CurrencyResponse>>
    {
        /// <inheritdoc />
        public Task<IEnumerable<CurrencyResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<CurrencyResponse> categories = Currency
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
