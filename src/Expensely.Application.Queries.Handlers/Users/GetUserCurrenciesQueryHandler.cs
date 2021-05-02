using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Queries.Processors.Users;
using Expensely.Application.Queries.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Users
{
    /// <summary>
    /// Represents the <see cref="GetUserCurrenciesQuery"/> handler.
    /// </summary>
    public sealed class GetUserCurrenciesQueryHandler
        : IQueryHandler<GetUserCurrenciesQuery, Maybe<IEnumerable<UserCurrencyResponse>>>
    {
        private readonly IGetUserCurrenciesQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCurrenciesQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get user currencies query processor.</param>
        public GetUserCurrenciesQueryHandler(IGetUserCurrenciesQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<IEnumerable<UserCurrencyResponse>>> Handle(
            GetUserCurrenciesQuery request,
            CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
