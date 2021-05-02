using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Currencies;
using Expensely.Application.Queries.Currencies;
using Expensely.Application.Queries.Processors.Currencies;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Handlers.Currencies
{
    /// <summary>
    /// Represents the <see cref="GetCurrenciesQuery"/> handler.
    /// </summary>
    public sealed class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IEnumerable<CurrencyResponse>>
    {
        private readonly IGetCurrenciesQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrenciesQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get currencies query processor.</param>
        public GetCurrenciesQueryHandler(IGetCurrenciesQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<IEnumerable<CurrencyResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
