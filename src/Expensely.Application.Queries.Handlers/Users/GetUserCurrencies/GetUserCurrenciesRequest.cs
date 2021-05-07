using System;

namespace Expensely.Application.Queries.Handlers.Users.GetUserCurrencies
{
    /// <summary>
    /// Represents the request for getting the user's currencies.
    /// </summary>
    public sealed record GetUserCurrenciesRequest(Ulid UserId);
}
