using System;
using System.Collections.Generic;
using Expensely.Application.Contracts.Users;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Users
{
    /// <summary>
    /// Represents the query for getting a user's currencies.
    /// </summary>
    public sealed class GetUserCurrenciesQuery : IQuery<IEnumerable<UserCurrencyResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCurrenciesQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public GetUserCurrenciesQuery(Ulid userId) => UserId = userId;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }
    }
}
