using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Handlers.Users.GetUserCurrencies;
using Expensely.Domain.Modules.Users;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Users
{
    /// <summary>
    /// Represents the data request for getting a user's currencies.
    /// </summary>
    public sealed class GetUserCurrenciesDataRequest : IGetUserCurrenciesDataRequest
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCurrenciesDataRequest"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public GetUserCurrenciesDataRequest(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<IEnumerable<UserCurrencyModel>> GetAsync(GetUserCurrenciesRequest request, CancellationToken cancellationToken = default)
        {
            var userCurrencies = await _session.Query<User>()
                .Where(x => x.Id == request.UserId.ToString())
                .Select(x => new
                {
                    Currencies = x.Currencies.Select(c => new UserCurrencyModel
                    {
                        Value = c.Value,
                        Name = c.Name,
                        Code = c.Code,
                        IsPrimary = c.Value == x.PrimaryCurrency.Value
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            return userCurrencies?.Currencies;
        }
    }
}
