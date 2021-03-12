using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Queries.Processors.Users;
using Expensely.Application.Queries.Users;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Users;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Users
{
    /// <summary>
    /// Represents the <see cref="GetUserCurrenciesQuery"/> processor.
    /// </summary>
    public sealed class GetUserCurrenciesQueryProcessor : IGetUserCurrenciesQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCurrenciesQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetUserCurrenciesQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<IReadOnlyCollection<UserCurrencyResponse>>> Process(
            GetUserCurrenciesQuery query,
            CancellationToken cancellationToken = default)
        {
            if (query.UserId != _userInformationProvider.UserId)
            {
                return Maybe<IReadOnlyCollection<UserCurrencyResponse>>.None;
            }

            var userCurrencies = await _session.Query<User>()
                .Where(x => x.Id == query.UserId)
                .Select(x => new
                {
                    x.PrimaryCurrency,
                    x.Currencies
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userCurrencies is null)
            {
                return Maybe<IReadOnlyCollection<UserCurrencyResponse>>.None;
            }

            UserCurrencyResponse[] userCurrencyResponses = userCurrencies.Currencies
                .Select(x => new UserCurrencyResponse
                {
                    Id = x.Value,
                    Name = x.Name,
                    Code = x.Code,
                    IsPrimary = x.Value == userCurrencies.PrimaryCurrency.Value
                })
                .OrderByDescending(x => x.IsPrimary)
                .ToArray();

            return userCurrencyResponses;
        }
    }
}
