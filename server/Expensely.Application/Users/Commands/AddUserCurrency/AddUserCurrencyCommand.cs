﻿using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.AddUserCurrency
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