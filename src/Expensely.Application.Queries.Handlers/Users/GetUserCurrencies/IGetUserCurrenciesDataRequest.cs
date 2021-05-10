using System.Collections.Generic;
using Expensely.Application.Queries.Handlers.Abstractions;

namespace Expensely.Application.Queries.Handlers.Users.GetUserCurrencies
{
    /// <summary>
    /// Represents the data request for getting a user's currencies interface.
    /// </summary>
    public interface IGetUserCurrenciesDataRequest : IDataRequest<GetUserCurrenciesRequest, IEnumerable<UserCurrencyModel>>
    {
    }
}
