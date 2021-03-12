using System;
using System.Collections.Generic;
using Expensely.Application.Contracts.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Users
{
    /// <summary>
    /// Represents the query for getting a user's currencies.
    /// </summary>
    public sealed class GetUserCurrenciesQuery : IQuery<Maybe<IReadOnlyCollection<UserCurrencyResponse>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCurrenciesQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public GetUserCurrenciesQuery(Guid userId) => UserId = userId.ToString();

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; }
    }
}
