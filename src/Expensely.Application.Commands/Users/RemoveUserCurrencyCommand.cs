using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for removing a currency from a user.
    /// </summary>
    public sealed class RemoveUserCurrencyCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        public RemoveUserCurrencyCommand(Ulid userId, int currency)
        {
            UserId = userId;
            Currency = currency;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }
    }
}
