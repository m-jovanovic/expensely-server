using System;
using Expensely.Common.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Users.RemoveUserCurrency
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
        public RemoveUserCurrencyCommand(Guid userId, int currency)
        {
            UserId = userId;
            Currency = currency;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }
    }
}
