using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for adding a currency to a user.
    /// </summary>
    public sealed class AddUserCurrencyCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserCurrencyCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        public AddUserCurrencyCommand(Guid userId, int currency)
        {
            UserId = userId.ToString();
            Currency = currency;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }
    }
}
